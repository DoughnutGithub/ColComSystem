using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.DB;
using CL.Interfaces;

namespace CL.Service
{
    /// <summary>
    /// 数据库连接管理
    /// </summary>
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private EFHelper _db;
        public EFHelper Get()
        {
            return _db ?? (_db = new EFHelper());
        }
        protected override void DisposeCore()
        {
            if (_db != null)
                _db.Dispose();
        }
    }

    /// <summary>
    /// 释放
    /// </summary>
    public class Disposable : IDisposable
    {
        private bool _isDisposed;
        ~Disposable()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                DisposeCore();
            }
            _isDisposed = true;
        }
        protected virtual void DisposeCore()
        {

        }
    }
}
