using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class DbConnectSource : IDisposable
    {
        private bool _isDisposed = false;

        private string _ConnectString = string.Empty;
        private OracleConnection _OracleConnection;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectString"></param>
        public DbConnectSource(string connectString)
        {
            _ConnectString = connectString;
        }

        /// <summary>
        /// 得到数据库连接
        /// </summary>
        public OracleConnection SqlConnection
        {
            get
            {
                if (null == _OracleConnection)
                    _OracleConnection = new OracleConnection(_ConnectString);
                return _OracleConnection;
            }
        }

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 由终结器调用以释放资源。
        /// </summary>
        ~DbConnectSource()
        {
            Dispose(true);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True/false</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (this._OracleConnection != null)
                    {
                        this._OracleConnection.Dispose();
                        this._OracleConnection = null;
                    }

                }

                _isDisposed = true;
            }
        }

        #endregion
    }
}
