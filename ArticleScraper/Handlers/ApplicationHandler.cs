using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using ArticleScraper.DataTypes;
using ArticleScraper.Utilities;
using ArticleScraper.ThirdParties;

namespace ArticleScraper
{
    public sealed class ApplicationHandler
    {
        #region variables

        /// <summary>
        /// for Object Instance
        /// </summary>
        private static ApplicationHandler _instance = null;
        private static readonly object _instanceLocker = new object();
        private static readonly object _loadingLocker = new object();

        //software information
        private string _applicationName;

        public string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        private string _applicationVersion;

        public string ApplicationVersion
        {
            get { return _applicationVersion; }
            set { _applicationVersion = value; }
        }

        private bool _isStopNotify = false;

        public bool IsStopNotify
        {
            get { return _isStopNotify; }
            set { _isStopNotify = value; }
        }        

        public bool doShowSplashScreen = true;
        public bool isLoadingCompleted = false;

        #endregion

        #region constructors

        public static ApplicationHandler Instance
        {
            get
            {
                lock (_instanceLocker)
                {
                    if (_instance == null)
                    {
                        _instance = new ApplicationHandler();
                    }
                }
                return _instance;
            }
        }

        ApplicationHandler()
        {
            try
            {
                //load all settings or user

                //finish loading all settings
                lock (_loadingLocker)
                    isLoadingCompleted = true;
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region load application settings



        #endregion

        #region save application settings



        #endregion

        #region public functions

        /// <summary>
        /// Do Something function
        /// </summary>
        public void DoSomething()
        {
            //not doing anything at all
        }

        /// <summary>
        /// Determine whether AppSettings has been loaded completely
        /// </summary>
        /// <returns></returns>
        public bool IsLoadingCompleled()
        {
            lock (_loadingLocker)
                return isLoadingCompleted;
        }

        #endregion

        #region utility functions



        #endregion
    }
}
