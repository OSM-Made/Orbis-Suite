using Microsoft.Extensions.Logging;
using OrbisLib2.Common.Database.Types;
using OrbisLib2.Common.Dispatcher;
using OrbisLib2.General;
using OrbisLib2.Targets;
using SimpleUI.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfHexaEditor.Core;
using System.Text;
using System.Windows.Input;

namespace OrbisPeeknPoke
{
    public record PeekInfo(ulong Address, ulong Offset, ulong Length);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SimpleWindow
    {
        private List<PeekInfo> JumpList = new();
        private PeekInfo CurrentPeek;

        public MainWindow()
        {
            InitializeComponent();

            var test = new byte[0x200];
            test[0] = 0xFF;
            test[1] = 0xFF;
            test[2] = 0xFF;
            test[3] = 0xFF;

            test[4] = 0x9A;
            test[5] = 0x02;


            test[12] = 0x00;
            test[13] = 0x00;
            test[14] = 0x8A;
            test[15] = 0x42;

            HexView.Stream = new MemoryStream(test);
        }

        public void Show(ILogger logger)
        {
            base.Show();

            DispatcherClient.Subscribe(logger);

            Events.ProcAttach += Events_ProcAttach;
            Events.ProcDetach += Events_ProcDetach;
            Events.ProcDie += Events_ProcDie;
            Events.TargetStateChanged += Events_TargetStateChanged;
            Events.DBTouched += Events_DBTouched;
            Events.SelectedTargetChanged += Events_SelectedTargetChanged;

            HexView.SelectionStartChanged += HexView_SelectionStartChanged;
            HexView.SelectionLengthChanged += HexView_SelectionLengthChanged;

            // Update State
            Task.Run(async () =>
            {
                if (TargetManager.SelectedTarget != null)
                {
                    await EnableProgram(await TargetManager.SelectedTarget.Debug.IsDebugging());
                }
            });
        }

        #region Events

        private void HexView_SelectionLengthChanged(object? sender, EventArgs e)
        {
            UpdateDataInspector();
            UpdateInfoBar();
        }

        private void HexView_SelectionStartChanged(object? sender, EventArgs e)
        {
            UpdateDataInspector();
            UpdateInfoBar();
        }

        private async Task EnableProgram(bool attached)
        {
            var currentTarget = TargetManager.SelectedTarget;
            if (currentTarget.Info.Status != TargetStatusType.APIAvailable)
                attached = false;

            Dispatcher.Invoke(() =>
            {
                SelectBase.IsEnabled = attached;
                Peek.IsEnabled = attached;
                Poke.IsEnabled = attached;

                Int8.IsEnabled = attached;
                UInt8.IsEnabled = attached;
                Int16.IsEnabled = attached;
                UInt16.IsEnabled = attached;
                Int32.IsEnabled = attached;
                UInt32.IsEnabled = attached;
                Int64.IsEnabled = attached;
                UInt64.IsEnabled = attached;
                Float.IsEnabled = attached;
                Double.IsEnabled = attached;
                String.IsEnabled = attached;
            });

            if (attached)
            {
                var baseAddress = await GetBaseAddress();
                Dispatcher.Invoke(() =>
                {
                    // Fill the base address for the first time.
                    if (BaseAddress.FieldText == string.Empty)
                    {
                        BaseAddress.FieldText = $"0x{baseAddress.ToString("X")}";
                    }

                    HexView.ReadOnlyMode = false;
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    BaseAddress.FieldText = string.Empty;
                    HexView.ReadOnlyMode = true;
                });
            }
        }

        private async void Events_TargetStateChanged(object? sender, TargetStateChangedEvent e)
        {
            if (e.Name != TargetManager.SelectedTarget.Name)
                return;

            switch (e.State)
            {
                case TargetStateChangedEvent.TargetState.APIAvailable:
                    await EnableProgram(await TargetManager.SelectedTarget.Debug.IsDebugging());
                    break;
                case TargetStateChangedEvent.TargetState.APIUnAvailable:
                    await EnableProgram(false);
                    break;
            }
        }

        private async void Events_ProcDie(object? sender, ProcDieEvent e)
        {
            // Only accept events for the selected target.
            if (e.SendingTarget.IPAddress != TargetManager.SelectedTarget.IPAddress)
                return;

            // Disable the attached options.
            await EnableProgram(false);
        }

        private async void Events_ProcDetach(object? sender, ProcDetachEvent e)
        {
            // Only accept events for the selected target.
            if (e.SendingTarget.IPAddress != TargetManager.SelectedTarget.IPAddress)
                return;

            // Disable the attached options.
            await EnableProgram(false);
        }

        private async void Events_ProcAttach(object? sender, ProcAttachEvent e)
        {
            // Only accept events for the selected target.
            if (e.SendingTarget.IPAddress != TargetManager.SelectedTarget.IPAddress)
                return;

            // Enable the attached options.
            await EnableProgram(true);
        }

        private async void Events_DBTouched(object? sender, DBTouchedEvent e)
        {
            await EnableProgram(await TargetManager.SelectedTarget.Debug.IsDebugging());
        }

        private async void Events_SelectedTargetChanged(object? sender, SelectedTargetChangedEvent e)
        {
            await EnableProgram(await TargetManager.SelectedTarget.Debug.IsDebugging());
        }

        #endregion

        #region Buttons

        private bool TryConvertStringToUlong(string str, out ulong val)
        {
            if (str.StartsWith("0x"))
            {
                if (!ulong.TryParse(str.Substring(2), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out val))
                {
                    return false;
                }
            }
            else
            {
                if (!ulong.TryParse(str, out val) && !ulong.TryParse(str, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out val))
                {
                    return false;
                }
            }

            return true;
        }

        private bool GetPeekPokeInfo(out ulong address, out ulong offset, out ulong length, bool displayErrors = true)
        {
            // Hex or decimal value of address
            if (!TryConvertStringToUlong(BaseAddress.FieldText, out address))
            {
                if (displayErrors)
                    SimpleMessageBox.ShowError(this, "Failed to parse Base Address please ensure that it is a valid hex or decimal number.", "Failed to parse Base Address.");

                offset = 0;
                length = 0;
                return false;
            }

            // Hex or decimal value of offset
            if (!TryConvertStringToUlong(Offset.FieldText, out offset))
            {
                if (displayErrors)
                    SimpleMessageBox.ShowError(this, "Failed to parse Offset please ensure that it is a valid hex or decimal number.", "Failed to parse Offset.");

                length = 0;
                return false;
            }

            // Hex or decimal value of length
            if (!TryConvertStringToUlong(Length.FieldText, out length))
            {
                if (displayErrors)
                    SimpleMessageBox.ShowError(this, "Failed to parse Length please ensure that it is a valid hex or decimal number.", "Failed to parse Length.");

                return false;
            }

            return true;
        }

        private async Task<ulong> GetBaseAddress()
        {
            (var result, var libraryList) = await TargetManager.SelectedTarget.Debug.GetLibraries();

            // Make sure we got the lib list.
            if (!result.Succeeded)
            {
                //SimpleMessageBox.ShowError(this, $"Couldn't get the process base: {result.ErrorMessage}", "Failed to get the process base.");
                return 0;
            }

            // Get the main app.
            var mainExecutable = libraryList.FirstOrDefault();
            if (mainExecutable == null)
            {
                // This really shouldn't occur.
                //SimpleMessageBox.ShowError(this, $"Couldn't get the process base", "Failed to get the process base.");
                return 0;
            }

            return mainExecutable.MapBase;
        }

        private async void SelectBase_Click(object sender, RoutedEventArgs e) => await Dispatcher.Invoke(async () => { BaseAddress.FieldText = $"0x{(await GetBaseAddress()).ToString("X")}"; });

        private async void Peek_Click(object sender, RoutedEventArgs e)
        {
            if (!GetPeekPokeInfo(out var address, out var offset, out var length))
                return;

            (var result, var data) = await TargetManager.SelectedTarget.Debug.ReadMemory(address + offset, length);

            // Make sure we peeked the memory.
            if (!result.Succeeded)
            {
                SimpleMessageBox.ShowError(this, $"We couldnt read the memory at 0x{(address + offset).ToString("X")}: {result.ErrorMessage}", "Failed to read the memory.");
                return;
            }

            // If we actually got the data update the hex box.
            if (data != null && data.Length > 0)
            {
                // Clear the jump list if were reading new memory.
                if (JumpList.Count > 0 && (address != CurrentPeek.Address || offset != CurrentPeek.Offset))
                {
                    JumpList.Clear();
                    //ReturnPointer.IsEnabled = false;
                }

                // Save this info for later so we know when we are peeking new memory.
                CurrentPeek = new(address, offset, length);

                HexView.Stream = new MemoryStream(data);
                HexView.ReadOnlyMode = false;

                CurrentAddress.Text = $"0x{address.ToString("X")}";
                UpdateInfoBar();
            }
        }

        private async void Poke_Click(object sender, RoutedEventArgs e)
        {
            if (!HexView.IsFileOrStreamLoaded)
                return;

            if (!GetPeekPokeInfo(out var address, out var offset, out var _))
                return;

            // Submit the changes to be written to the stream.
            var tempData = HexView.GetAllBytes(true);

            // Write the changes to the process.
            var result = await TargetManager.SelectedTarget.Debug.WriteMemory(address + offset, tempData);

            // Make sure we poked the memory.
            if (!result.Succeeded)
            {
                SimpleMessageBox.ShowError(this, $"We couldnt write the memory at 0x{(address + offset).ToString("X")}: {result.ErrorMessage}", "Failed to write the memory.");
                return;
            }

            HexView.Stream = new MemoryStream(tempData);
        }

        #endregion

        #region Context Menu

        private void CopyHex_Click(object sender, RoutedEventArgs e)
        {
            HexView.CopyToClipboard(CopyPasteMode.HexaString);
        }

        private void CopyText_Click(object sender, RoutedEventArgs e)
        {
            HexView.CopyToClipboard(CopyPasteMode.AsciiString);
        }

        private void CopyCSharp_Click(object sender, RoutedEventArgs e)
        {
            HexView.CopyToClipboard(CopyPasteMode.CSharpCode);
        }

        private void CopyCPP_Click(object sender, RoutedEventArgs e)
        {
            HexView.CopyToClipboard(CopyPasteMode.CCode);
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            HexView.SelectAll();
        }

        private async void FollowPointer_Click(object sender, RoutedEventArgs e)
        {
            if (!GetPeekPokeInfo(out var lastAddress, out var offset, out var length))
                return;

            byte[] RawJumpAddress = new byte[8];

            // Grab 8 bytes
            Array.Copy(((MemoryStream)HexView.Stream).ToArray(), HexView.SelectionStart, RawJumpAddress, 0, 8);

            ulong address;
            try
            {
                address = BitConverter.ToUInt64(RawJumpAddress, 0);
            }
            catch
            {
                SimpleMessageBox.ShowError(this, "The memory was not a Pointer.", "The memory was not a Pointer.");
                return;
            }

            (var result, var data) = await TargetManager.SelectedTarget.Debug.ReadMemory(address, length);

            // Make sure we peeked the memory.
            if (!result.Succeeded)
            {
                Dispatcher.Invoke(() => SimpleMessageBox.ShowError(this, $"We couldnt read the memory at 0x{address.ToString("X")}: {result.ErrorMessage}", "Failed to read the memory."));
                return;
            }

            // If we actually got the data update the hex box.
            if (data != null && data.Length > 0)
            {
                // Add the last address to the list.
                JumpList.Add(new PeekInfo(lastAddress, offset, length));

                // Add our jump to the pointer to the current peek info.
                CurrentPeek = new(address, 0, length);

                BaseAddress.FieldText = $"0x{address.ToString("X")}";
                Offset.FieldText = $"0x0";

                HexView.Stream = new MemoryStream(data);

                UpdateInfoBar();
            }
        }

        private async void ReturnPointer_Click(object sender, RoutedEventArgs e)
        {
            if (JumpList.Count == 0)
            {
                SimpleMessageBox.ShowError(this, $"We have no where else to jump to.", "Jump list is empty.");
                return;
            }

            var lastJump = JumpList.Last();

            (var result, var data) = await TargetManager.SelectedTarget.Debug.ReadMemory(lastJump.Address + lastJump.Offset, lastJump.Length);

            // Make sure we peeked the memory.
            if (!result.Succeeded)
            {
                SimpleMessageBox.ShowError(this, $"We couldnt read the memory at 0x{lastJump.Address + lastJump.Offset}: {result.ErrorMessage}", "Failed to read the memory.");
                return;
            }

            // If we actually got the data update the hex box.
            if (data != null && data.Length > 0)
            {
                // Add the last address to the list.
                JumpList.Remove(lastJump);

                // Add our jump to the pointer to the current peek info.
                CurrentPeek = new(lastJump.Address, lastJump.Offset, lastJump.Length);

                BaseAddress.FieldText = $"0x{lastJump.Address.ToString("X")}";
                Offset.FieldText = $"0x{lastJump.Offset.ToString("X")}";
                Length.FieldText = $"0x{lastJump.Length.ToString("X")}";

                HexView.Stream = new MemoryStream(data);

                UpdateInfoBar();
            }
        }

        #endregion

        #region Data Inspector

        public int FindIndexOfFirstNonCharacter(int startIndex, byte[] byteArray)
        {
            for (int i = startIndex; i < byteArray.Length; i++)
            {
                if (!IsCharacter(byteArray[i]))
                {
                    return i;
                }
            }

            return -1; // Return -1 if all bytes are characters
        }

        public bool IsCharacter(byte b)
        {
            // Check if the byte represents a character (letter, number, symbol)
            return (b >= 32 && b <= 126); // You can adjust this range based on your requirements
        }

        private void UpdateStringInspector(byte[] fullData)
        {
            int startIndex = (int)HexView.SelectionStart;
            int endIndex = FindIndexOfFirstNonCharacter(startIndex, fullData);

            if (endIndex == -1)
            {
                String.FieldText = "Invalid";
                return;
            }

            int maxLength = Math.Min(endIndex - startIndex, 50); // Limit the maximum length to 50 bytes

            byte[] substringBytes = new byte[maxLength];
            Array.Copy(fullData, startIndex, substringBytes, 0, maxLength);

            String.FieldText = Encoding.UTF8.GetString(substringBytes, 0, maxLength);
        }

        private void UpdateDataInspector()
        {
            if (!HexView.IsFileOrStreamLoaded)
                return;

            // Use the selection length unless the start and stop are the same location then just 1.
            var dataSize = HexView.SelectionLength;
            if (HexView.SelectionStart == HexView.SelectionStop)
                dataSize = 8;

            // Limit to the amount of data we have left if we are at thee end of the stream.
            if (HexView.SelectionStart + dataSize > HexView.Stream.Length)
                dataSize = HexView.Stream.Length - HexView.SelectionStart;

            // Make sure we actually have some data.
            if (dataSize <= 0)
                return;

            // Copy the data to a temporary array.
            var tempData = new byte[dataSize];
            var fullData = HexView.GetAllBytes(true);
            Array.Copy(fullData, HexView.SelectionStart, tempData, 0, dataSize);

            // Parse out the bytes should always be able to do this.
            Int8.FieldText = ((sbyte)tempData[0]).ToString("D");
            UInt8.FieldText = tempData[0].ToString("D");

            // If the data length is big enough for this type try to parse it.
            if (tempData.Length >= 2)
            {
                Int16.FieldText = BitConverter.ToInt16(tempData).ToString("D");
                UInt16.FieldText = BitConverter.ToUInt16(tempData).ToString("D");
            }
            else
            {
                Int16.FieldText = "Invalid";
                UInt16.FieldText = "Invalid";
            }

            // If the data length is big enough for this type try to parse it.
            if (tempData.Length >= 4)
            {
                Int32.FieldText = BitConverter.ToInt32(tempData).ToString("D");
                UInt32.FieldText = BitConverter.ToUInt32(tempData).ToString("D");

                var tempFloat = BitConverter.ToSingle(tempData);
                if (tempFloat < 0.0000000001 || tempFloat > 1000000000000.0)
                    Float.FieldText = tempFloat.ToString("0.###############E+0");
                else
                    Float.FieldText = tempFloat.ToString("F");
            }
            else
            {
                Int32.FieldText = "Invalid";
                UInt32.FieldText = "Invalid";
                Float.FieldText = "Invalid";
            }

            // If the data length is big enough for this type try to parse it.
            if (tempData.Length >= 8)
            {
                Int64.FieldText = BitConverter.ToInt64(tempData).ToString("D");
                UInt64.FieldText = BitConverter.ToUInt64(tempData).ToString("D");

                var tempDouble = BitConverter.ToDouble(tempData);
                if (tempDouble < 0.0000000001 || tempDouble > 1000000000000.0)
                    Double.FieldText = tempDouble.ToString("0.##############################E+0");
                else
                    Double.FieldText = tempDouble.ToString("F");
            }
            else
            {
                Int64.FieldText = "Invalid";
                UInt64.FieldText = "Invalid";
                Double.FieldText = "Invalid";
            }

            UpdateStringInspector(fullData);
        }

        private void String_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!HexView.IsFileOrStreamLoaded)
                return;

            var selectionLocation = HexView.SelectionStart;
            if (selectionLocation == -1)
                return;

            if (e.Key == Key.Enter)
            {
                var rawData = HexView.GetAllBytes(true);
                var stringBtyes = Encoding.UTF8.GetBytes(String.FieldText + '\0');

                Array.Copy(stringBtyes, 0, rawData, selectionLocation, stringBtyes.Length);
                HexView.Stream = new MemoryStream(rawData);
                HexView.SelectionStart = selectionLocation;
            }
        }

        #endregion

        #region Info Footer

        private void UpdateInfoBar()
        {
            // Update the curent selection address.
            if (GetPeekPokeInfo(out var address, out var offset, out var _, false))
                CurrentAddress.Text = $"0x{((ulong)HexView.SelectionStart + address + offset).ToString("X")}";
            else
                CurrentAddress.Text = "0x0";

            // Update the current offset in the hex view.
            OffsetValue.Text = $"0x{HexView.SelectionStart.ToString("X")}";

            // Set the Selection Length
            if (HexView.SelectionStart == HexView.SelectionStop)
                SelectionLength.Text = "0x0";
            else
                SelectionLength.Text = $"0x{HexView.SelectionLength.ToString("X")}";
        }

        #endregion

    }
}
