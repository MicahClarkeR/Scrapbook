using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Persistence
{
    /// <summary>
    /// Wrapper designed to make interacting with an underlying Directory easier.
    /// </summary>
    public abstract class ZipperDirectoryWrapper
    {
        /// <summary>
        /// Directory being wrapped.
        /// </summary>
        public readonly ZipperDirectory Directory;

        /// <summary>
        /// Sets up the wrapper to wrap the passed Zipper Directory.
        /// </summary>
        /// <param name="directory"></param>
        public ZipperDirectoryWrapper(ZipperDirectory directory)
        {
            Directory = directory;

            Initialise();
        }

        /// <summary>
        /// Initialise this Wrapper, called when the constructor is initalised.
        /// </summary>
        protected virtual void Initialise()
        {

        }

        /// <summary>
        /// Prepares to save the contained Zipper Directory.
        /// </summary>
        public void PrepareToSave() => Directory.PrepareToSave();
    }
}
