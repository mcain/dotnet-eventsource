﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace LaunchDarkly.EventSource
{
    internal interface IStreamReader : IDisposable
    {
        Task<string> ReadLineAsync();
    }

    internal class EventSourceStreamReader : IStreamReader
    {
        private StreamReader _streamReader;

        public EventSourceStreamReader(Stream stream)
        {
            _streamReader = new StreamReader(stream);
        }

        public virtual async Task<string> ReadLineAsync()
        {
            try
            {
                await Task.Yield();
                return await _streamReader.ReadLineAsync();
            }
            catch (ObjectDisposedException)
            {
                // Do nothing            
            }

            return null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                _streamReader?.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // Do nothing...
            }
            finally
            {
                _streamReader = null;
            }
        }
    }
}
