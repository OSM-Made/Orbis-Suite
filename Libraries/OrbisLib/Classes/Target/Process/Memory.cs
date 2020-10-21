using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite
{
    public class Memory
    {
        private Target Target;
        private Process Process;
        public Extension Ext;

        public Memory(Target Target, Process Process)
        {
            this.Target = Target;
            this.Process = Process;

            Ext = new Extension(Target, this);
        }
    }
}
