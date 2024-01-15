using OrbisSuiteService.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.ServiceProcess;
using NetFwTypeLib;
using OrbisLib2.Common;

#if DEBUG
var service = new Service();
service.OnStartPublic(new string[0]);
#else
ServiceBase.Run(new Service());
#endif

class Service : ServiceBase
{
    private ServiceProvider _serviceProvider;
#if DEBUG
    bool RunService = true;
#endif

    public Service()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddLogging(builder => 
        {
            builder.AddSimpleConsole(options =>
            {
                options.IncludeScopes = false;
                options.SingleLine = true;
                options.TimestampFormat = "HH:mm:ss ";
            });
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    public void OnStartPublic(string[] args)
    {
#if DEBUG

#else
        // setup up rule in firewall for event listener.
        INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
        List<INetFwRule> firewallRules = firewallPolicy.Rules.OfType<INetFwRule>().Where(x => x.Name.Contains("OrbisSuiteEvents")).ToList();

        // Only add the rule when it does not exist.
        if(firewallRules.Count <= 0) 
        {
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRule.Description = "Allow for events to be sent from target PS4 to windows service.";
            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            firewallRule.Enabled = true;
            firewallRule.InterfaceTypes = "All";
            firewallRule.Name = "OrbisSuiteEvents";
            firewallRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            firewallRule.LocalPorts = Config.EventPort.ToString();
            firewallPolicy.Rules.Add(firewallRule);
        }
#endif

        // Create logger instance.
        var logger = _serviceProvider.GetService<ILoggerFactory>() 
            .AddFile(Config.OrbisPath + @"\Logging\OrbisServiceLog.txt")
            .CreateLogger<Program>();

        var dp = new Dispatcher(logger);
#if DEBUG
        while (RunService) {  Thread.Sleep(10); }
#endif
    }

    protected override void OnStart(string[] args)
    {
        OnStartPublic(args);
    }
    protected override void OnStop()
    {
        Console.WriteLine("Stopping");
#if DEBUG
        RunService = false;
#endif
    }
    protected override void OnPause()
    {
        Console.WriteLine("Pausing");
    }
}