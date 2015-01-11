using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleScraper.DataTypes
{
    public class SystemProxy
    {
        #region variables

        private string _proxyIp;

        public string ProxyIp
        {
            get { return _proxyIp; }
            set { _proxyIp = value; }
        }

        private string _proxyPort;

        public string ProxyPort
        {
            get { return _proxyPort; }
            set { _proxyPort = value; }
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private bool _isPrivate;

        public bool IsPrivate
        {
            get { return _isPrivate; }
            set { _isPrivate = value; }
        }

        private bool _isAlive;

        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

        private bool _isAvailable;

        public bool IsAvailable
        {
            get { return _isAvailable; }
            set { _isAvailable = value; }
        }

        #endregion

        #region constructors

        public SystemProxy()
            : this("", "", "", "", false, false, true)
        {
        }

        public SystemProxy(string ip, string port)
            : this(ip, port, "", "", false, false, true)
        {
        }

        public SystemProxy(string ip, string port, string username, string password)
            : this(ip, port, username, password, true, false, true)
        {
        }

        public SystemProxy(string ip, string port, string username, string password, bool isPrivate, bool isAlive, bool isAvailable)
        {
            this._proxyIp = ip;
            this._proxyPort = port;
            this._username = username;
            this._password = password;
            this._isAlive = isAlive;
            this._isAvailable = isAvailable;
            this._isPrivate = isPrivate;
        }

        #endregion

        #region utility Methods

        public override string ToString()
        {
            if(!this._isPrivate)
                return String.Format("{0}:{1}", _proxyIp, _proxyPort);
            else
                return String.Format("{0}:{1}:{2}:{3}", _proxyIp, _proxyPort, _username, _password);
        }

        #endregion
    }
}
