using System;

namespace Library
{
    public abstract class DisposableObject : IDisposable
    {
        private bool _isDisposed;

        ~DisposableObject()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            try
            {
                Dispose(true);
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (disposing)
                {
                    DisposeImpl();
                }
            }
        }

        protected virtual void DisposeImpl()
        {
        }

        protected void EnsureIsNotDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(ToString());
            }
        }
    }
}
