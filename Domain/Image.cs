﻿using EasyData.EntityFrameworkCore;

namespace Domain
{
    /// <summary>
    /// Local stored image.
    /// </summary>
    public record Image
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Name and extension of file.
        /// </summary>
        public string Name => System.IO.Path.GetFileName(Path);

        /// <summary>
        /// Path to image.
        /// </summary>
        [MetaEntityAttr(ShowOnCreate = false, ShowOnEdit = false)]
        public string Path { get; init; }
    }
}