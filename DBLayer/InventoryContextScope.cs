using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    

    public class InventoryContextScope : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false;
        public InventoryContext _context;


        public void TurnLazyLoadingOff()
        {
            if (_context != null)
            {
                _context.Configuration.LazyLoadingEnabled = false;
            }
        }

        public void TurnLazyLoadingOn()
        {
            if (_context != null)
            {
                _context.Configuration.LazyLoadingEnabled = true;
            }
        }

        public InventoryContext Create()
        {
            if (_context == null)
            {
                _context = new InventoryContext();
            }

            return _context;
        }


        public async Task Complete()
        {
            if (_context == null)
            {
                throw new InvalidOperationException("No context in memory.");
            }
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {

                if (_context != null)
                    _context.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
            Dispose(true);
        }
        #endregion
    }
}
