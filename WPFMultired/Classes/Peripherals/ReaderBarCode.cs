using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//     @
//    <))>
//    _/\_
namespace WPFMultired.Classes.Peripherals
{
    public class ReaderBarCode
    {
        public Action<string> callbackOut;//Calback para cuando sale cieerta cantidad del dinero

        public Action<string> callbackError;//Calback de error

        private SerialPort _serialBarCodeReader;

        public ReaderBarCode()
        {
            if (_serialBarCodeReader == null)
            {
                _serialBarCodeReader = new SerialPort();
            }
        }

        public void Start(string portReaderBarCode, int barcodeBaudRate)
        {
            try
            {
                if (_serialBarCodeReader != null)
                {
                    InitializePortBarcode(portReaderBarCode, barcodeBaudRate);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void InitializePortBarcode(string portName, int barcodeBaudRate)
        {
            try
            {
                if (!_serialBarCodeReader.IsOpen)
                {
                    _serialBarCodeReader.PortName = portName;
                    _serialBarCodeReader.BaudRate = barcodeBaudRate;
                    _serialBarCodeReader.Open();
                    _serialBarCodeReader.ReadTimeout = 200;
                    _serialBarCodeReader.DtrEnable = true;
                    _serialBarCodeReader.RtsEnable = true;
                    _serialBarCodeReader.DataReceived += new SerialDataReceivedEventHandler(_readerBarcode_DataReceived);
                }
            }
            catch (Exception ex)
            {
                callbackError?.Invoke(ex.Message);
            }
        }

        private void _readerBarcode_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(1000);
                string response = _serialBarCodeReader.ReadExisting();
                if (!string.IsNullOrEmpty(response))
                {
                    ProcessResponseBarcode(response);
                }
            }
            catch (Exception ex)
            {
                callbackError?.Invoke(ex.ToString());
            }
        }

        private void ProcessResponseBarcode(string response)
        {
            try
            {
                callbackOut?.Invoke(response);
            }
            catch (Exception ex)
            {

            }
        }

        public void Stop()
        {
            try
            {
                if (_serialBarCodeReader.IsOpen)
                {
                    _serialBarCodeReader.Close();
                }
            }
            catch (Exception ex)
            {
                callbackError?.Invoke(ex.ToString());
            }
        }
    }
}
