using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading;

namespace ArticleScraper
{
    public sealed class ApplicationMessageHandler
    {
        #region variables

        //instance & locker
        private static ApplicationMessageHandler _instance = null;
        private static readonly object _instanceLocker = new object();

        //using variables
        private static readonly object _usingLocker = new object();
        private Queue<string> _messages = new Queue<string>();
        private string _datetimeFormat = "dd-MM HH:mm:ss";
        private int _interval;

        //window controls
        private Window _currentWD;

        public Window CurrentWD
        {
            get { return _currentWD; }
        }

        private TextBox _messageTB;

        public TextBox MessageTB
        {
            get { return _messageTB; }
        }

        private Action<Window, TextBox, string> _updateTextBox;

        public Action<Window, TextBox, string> UpdateTextBox
        {
            get { return _updateTextBox; }
        }

        //for threading
        private Thread _worker;
        private bool _isRunning = false;
        public ManualResetEvent _signal = new ManualResetEvent(false);
        
        private bool _stopApplicationMessageHandler;
        private static readonly object _stopApplicationMessageHandlerLocker = new object();

        #endregion

        #region constructors

        public static ApplicationMessageHandler Instance
        {
            get
            {
                lock (_instanceLocker)
                {
                    if (_instance == null)
                    {
                        _instance = new ApplicationMessageHandler();
                    }
                }
                return _instance;
            }
        }

        ApplicationMessageHandler()
        {
            _interval = 20;
            _messageTB = new TextBox();
            _messages = new Queue<string>();            
            _stopApplicationMessageHandler = false;
            _worker = new Thread(worker_DoWork);
            _worker.IsBackground = true;
        }

        #endregion

        #region private functions

        /// <summary>
        /// ApplicationMessageHandler worker doWork function
        /// </summary>
        private void worker_DoWork()
        {
            do
            {
                _signal.WaitOne();
                lock (_stopApplicationMessageHandlerLocker)
                {
                    if (_stopApplicationMessageHandler)
                        break;
                }

                string message = string.Empty;

                lock (_usingLocker)
                {
                    if (_messages.Count > 0)
                        message = _messages.Dequeue();
                }

                if (message != string.Empty)
                    _updateTextBox.Invoke(_currentWD, _messageTB, CreateMessage(message));

                Thread.Sleep(_interval);

            } while (true);


        }

        /// <summary>
        /// ApplicationMessageHandler worker doWorkCompleted function
        /// </summary>
        private void worker_DoWorkCompleted()
        {
            _isRunning = false;
        }

        #endregion

        #region public functions

        /// <summary>
        /// Register controls to handler
        /// </summary>
        /// <param name="wd"></param>
        /// <param name="tb"></param>
        /// <param name="action"></param>
        public void RegisterHandler(Window wd, TextBox tb, Action<Window, TextBox, string> action)
        {
            _currentWD = wd;
            _messageTB = tb;
            _updateTextBox = action;
            _signal.Set();
        }

        /// <summary>
        /// Start handler
        /// </summary>
        public void StartHandler()
        {            
            lock (_stopApplicationMessageHandlerLocker)
            {
                _stopApplicationMessageHandler = false;
                _isRunning = true;
            }
            _worker.Start();
        }

        /// <summary>
        /// Stop handler
        /// </summary>
        public void StopHandler()
        {
            lock (_stopApplicationMessageHandlerLocker)
                _stopApplicationMessageHandler = true;
        }

        /// <summary>
        /// Abort handler
        /// </summary>
        public void AbortHandler()
        {
            lock (_stopApplicationMessageHandlerLocker)
            {
                _worker.Abort();
                _stopApplicationMessageHandler = true;
                _isRunning = false;
            }
        }

        /// <summary>
        /// Pause handler
        /// </summary>
        public void PauseHandler()
        {
            _signal.Reset();
        }

        /// <summary>
        /// Resume handler
        /// </summary>
        public void ResumeHandler()
        {
            _signal.Set();
        }

        /// <summary>
        /// Add message to queue
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(string message)
        {
            lock (_usingLocker)
                _messages.Enqueue(message);
        }

        public void Initialize()
        {

        }

        /// <summary>
        /// Check whether ApplicationStatisticHandler has stopped working
        /// </summary>
        /// <returns></returns>
        public bool IsStoppingCompleted()
        {
            return _isRunning ? false : true;
        }

        #endregion

        #region utility functions

        /// <summary>
        /// Create message from a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CreateMessage(string input)
        {
            return DateTime.Now.ToString(_datetimeFormat) + " : " + input;
        }

        #endregion
    }
}
