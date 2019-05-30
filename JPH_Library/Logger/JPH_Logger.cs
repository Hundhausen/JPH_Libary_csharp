#region Header

//----------------------------------------------------------------------
// 
// Project is available at https://github.com/Hundhausen
// This Project is licensed under the GNU General Public License v3.0
//
// Date: 2019-02-09
// User: Hundhausen
//
//----------------------------------------------------------------------

#endregion

using NLog;

namespace JPH_Library.Logger
{

    public class JPH_Logger
    {
        #region Var

        private readonly NLog.Logger _nLog;

        private static string _sPathToConfig = "";
        /// <summary>Gets or sets the Path to the NLog Configuration File</summary>
        /// <remarks>This can only be done once!</remarks>
        public static string PathToConfig
        {
            get => _sPathToConfig;
            set
            {
                if (string.IsNullOrEmpty(_sPathToConfig)) _sPathToConfig = value;
            }
        }

        #endregion

        #region Ctor

        public JPH_Logger(string sLoggerIn)
        {
            if (string.IsNullOrEmpty(_sPathToConfig)) return;
            NLog.Config.LoggingConfiguration config = new NLog.Config.XmlLoggingConfiguration(_sPathToConfig);
            NLog.LogManager.Configuration = config;
            _nLog = LogManager.GetLogger(sLoggerIn);
        }

        #endregion

        #region Functions

        /// <summary>
        /// For trace debugging, eg. "Method X start, Method X end"
        /// </summary>
        /// <param name="sMessageIn">What Message should be written</param>
        public void Trace(string sMessageIn)
        {
            _nLog?.Trace(sMessageIn);
        }

        /// <summary>
        /// For debugging, eg. session started, authentication failed
        /// </summary>
        /// <param name="sMessageIn">What Message should be written</param>
        public void Debug(string sMessageIn)
        {
            _nLog?.Debug(sMessageIn);
        }

        /// <summary>
        /// Normal behavior like mail sent, user updated profile etc.
        /// </summary>
        /// <param name="sMessageIn">What Message should be written</param>
        public void Info(string sMessageIn)
        {
            _nLog?.Info(sMessageIn);
        }

        /// <summary>
        /// Something unexpected, application will continue
        /// </summary>
        /// <param name="sMessageIn">What Message should be written</param>
        public void Warn(string sMessageIn)
        {
            _nLog?.Warn(sMessageIn);
        }

        /// <summary>
        /// Something failed; application may or may not continue
        /// </summary>
        /// <param name="sMessageIn">What Message should be written</param>
        public void Error(string sMessageIn)
        {
            _nLog?.Error(sMessageIn);
        }

        /// <summary>
        /// Something bad happened, program will stop
        /// </summary>
        /// <param name="sMessageIn">What Message should be written</param>
        public void Fatal(string sMessageIn)
        {
            _nLog?.Fatal(sMessageIn);
        }

        #endregion

    }
}

//----------------------------------------------------------------------
// Project is available at https://github.com/Hundhausen
//----------------------------------------------------------------------