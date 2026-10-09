using Scrapbook.Persistence;
using System.Collections.ObjectModel;

using System.Reflection;

/// <summary>
/// Represents a directory containing named files and subdirectories
/// within a Zipper content structure.
/// </summary>
/// <remarks>
/// Provides methods for adding, updating, removing, retrieving, and
/// creating content within the directory.
/// 
/// A directory can contain any object implementing
/// <see cref="IZipperContent"/>, including nested
/// <see cref="ZipperDirectory"/> instances.
/// 
/// Before saving, <see cref="PrepareToSave"/> is called to prepare
/// the directory and its contents for serialization.
/// </remarks>
public class ZipperDirectory : IZipperContent
{
    /// <summary>
    /// Occurs when the directory is about to prepare its contents for saving.
    /// </summary>
    /// <remarks>
    /// Raised before <see cref="PrepareToSave"/> is called on
    /// the directory's contained items.
    /// </remarks>
    public event EventHandler<EventArgs>? OnPreparingToSave;

    /// <summary>
    /// Gets the content stored in this directory, indexed by name.
    /// </summary>
    /// <remarks>
    /// The returned dictionary is read-only, but its contained
    /// objects can still be modified.
    /// </remarks>
    public ReadOnlyDictionary<string, IZipperContent> Content
        => new ReadOnlyDictionary<string, IZipperContent>(_content);

    private readonly IDictionary<string, IZipperContent> _content
        = new Dictionary<string, IZipperContent>();

    /// <summary>
    /// Gets the name of this directory.
    /// </summary>
    public string Name => _name;

    private string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZipperDirectory"/> class.
    /// </summary>
    /// <param name="name">The name of the directory.</param>
    /// <remarks>
    /// Calls <see cref="Initalise"/> after assigning the directory name.
    /// </remarks>
    public ZipperDirectory(string name)
    {
        _name = name;

        Initalise();
    }

    /// <summary>
    /// Adds or updates content using its own name as the key.
    /// </summary>
    /// <param name="content">The content to add or update.</param>
    public void AddOrUpdate(IZipperContent content)
        => AddOrUpdate(content.Name, content);

    /// <summary>
    /// Adds or updates content using a GUID as the key.
    /// </summary>
    /// <param name="id">The identifier of the content.</param>
    /// <param name="content">The content to add or update.</param>
    public void AddOrUpdate(Guid id, IZipperContent content)
        => AddOrUpdate(id.ToString(), content);

    /// <summary>
    /// Adds content under the specified name, or replaces existing
    /// content with the same name.
    /// </summary>
    /// <param name="name">The name used to identify the content.</param>
    /// <param name="content">The content to store.</param>
    public void AddOrUpdate(string name, IZipperContent content)
    {
        bool contains = Update(name, content);

        if (!contains)
            Add(name, content);
    }

    /// <summary>
    /// Updates existing content using its own name as the key.
    /// </summary>
    /// <param name="content">The replacement content.</param>
    /// <returns>
    /// <see langword="true"/> if the content was updated;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool Update(IZipperContent content)
        => Update(content.Name, content);

    /// <summary>
    /// Updates existing content identified by a GUID.
    /// </summary>
    /// <param name="id">The identifier of the content to update.</param>
    /// <param name="content">The replacement content.</param>
    /// <returns>
    /// <see langword="true"/> if matching content was found and updated;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool Update(Guid id, IZipperContent content)
        => Update(id.ToString(), content);

    /// <summary>
    /// Replaces existing content stored under the specified name.
    /// </summary>
    /// <param name="name">The name of the content to update.</param>
    /// <param name="content">The replacement content.</param>
    /// <returns>
    /// <see langword="true"/> if matching content was found and replaced;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// No content is added if <paramref name="name"/> does not exist.
    /// Use <see cref="AddOrUpdate(string, IZipperContent)"/>
    /// to insert content when necessary.
    /// </remarks>
    public bool Update(string name, IZipperContent content)
    {
        bool contains = _content.ContainsKey(name);

        if (contains)
            _content[name] = content;

        return contains;
    }

    /// <summary>
    /// Adds content using its own name as the key.
    /// </summary>
    /// <param name="content">The content to add.</param>
    public void Add(IZipperContent content)
        => Add(content.Name, content);

    /// <summary>
    /// Adds content identified by a GUID.
    /// </summary>
    /// <param name="id">The identifier of the content.</param>
    /// <param name="content">The content to add.</param>
    public void Add(Guid id, IZipperContent content)
        => Add(id.ToString(), content);

    /// <summary>
    /// Adds content under the specified name.
    /// </summary>
    /// <param name="name">The name used to identify the content.</param>
    /// <param name="content">The content to add.</param>
    /// <exception cref="ArgumentException">
    /// Content with the specified name already exists.
    /// </exception>
    public void Add(string name, IZipperContent content)
        => _content.Add(name, content);

    /// <summary>
    /// Removes content using its own name as the key.
    /// </summary>
    /// <param name="content">The content whose name identifies the entry to remove.</param>
    public void Remove(IZipperContent content)
        => Remove(content.Name);

    /// <summary>
    /// Removes content identified by a GUID.
    /// </summary>
    /// <param name="id">The identifier of the content to remove.</param>
    public void Remove(Guid id)
        => Remove(id.ToString());

    /// <summary>
    /// Removes content stored under the specified name.
    /// </summary>
    /// <param name="name">The name of the content to remove.</param>
    /// <remarks>
    /// Has no effect if the specified name does not exist.
    /// </remarks>
    public void Remove(string name)
        => _content.Remove(name);

    /// <summary>
    /// Creates a typed wrapper around a contained directory.
    /// </summary>
    /// <typeparam name="T">The type of wrapper to create.</typeparam>
    /// <param name="directory">
    /// The directory whose name identifies the contained directory to wrap.
    /// </param>
    /// <returns>
    /// The created wrapper, or <see langword="null"/> if no matching
    /// directory exists or a suitable constructor is unavailable.
    /// </returns>
    public T? WrapDirectory<T>(ZipperDirectory directory)
        where T : ZipperDirectoryWrapper
        => WrapDirectory<T>(directory.Name);

    /// <summary>
    /// Creates a typed wrapper around a directory identified by a GUID.
    /// </summary>
    /// <typeparam name="T">The type of wrapper to create.</typeparam>
    /// <param name="id">The identifier of the directory to wrap.</param>
    /// <returns>
    /// The created wrapper, or <see langword="null"/> if the directory
    /// cannot be found or the wrapper cannot be constructed.
    /// </returns>
    public T? WrapDirectory<T>(Guid id)
        where T : ZipperDirectoryWrapper
        => WrapDirectory<T>(id.ToString());

    /// <summary>
    /// Creates a typed wrapper around a contained directory.
    /// </summary>
    /// <typeparam name="T">
    /// The wrapper type, which must inherit from
    /// <see cref="ZipperDirectoryWrapper"/>.
    /// </typeparam>
    /// <param name="name">The name of the directory to wrap.</param>
    /// <returns>
    /// A new instance of <typeparamref name="T"/> wrapping the directory,
    /// or <see langword="null"/> if the directory is not found,
    /// the content is not a directory, or the wrapper type lacks
    /// a suitable constructor.
    /// </returns>
    /// <remarks>
    /// The wrapper type must declare a public constructor accepting
    /// a single <see cref="ZipperDirectory"/> parameter.
    /// The wrapper is instantiated using reflection.
    /// </remarks>
    /// <exception cref="TargetInvocationException">
    /// The wrapper's constructor throws an exception.
    /// </exception>
    public T? WrapDirectory<T>(string name)
        where T : ZipperDirectoryWrapper
    {
        ConstructorInfo? constructor =
            typeof(T).GetConstructor([typeof(ZipperDirectory)]);

        if (constructor == null)
            return null;

        if (!_content.TryGetValue(name, out IZipperContent? foundContent) ||
            foundContent == null ||
            foundContent is not ZipperDirectory directory)
            return null;

        object created = constructor.Invoke([directory]);
        T wrapper = (T)created;

        return wrapper;
    }

    /// <summary>
    /// Gets a contained directory of the specified type by GUID.
    /// </summary>
    /// <typeparam name="T">The expected directory type.</typeparam>
    /// <param name="id">The identifier of the directory.</param>
    /// <returns>The directory cast to <typeparamref name="T"/>.</returns>
    /// <exception cref="KeyNotFoundException">
    /// No content exists under the specified identifier.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// The stored content is not compatible with <typeparamref name="T"/>.
    /// </exception>
    public T GetDirectory<T>(Guid id) where T : ZipperDirectory
        => GetDirectory<T>(id.ToString());

    /// <summary>
    /// Gets a contained directory of the specified type by name.
    /// </summary>
    /// <typeparam name="T">The expected directory type.</typeparam>
    /// <param name="name">The name of the directory.</param>
    /// <returns>The directory cast to <typeparamref name="T"/>.</returns>
    /// <exception cref="KeyNotFoundException">
    /// No content exists under the specified name.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// The stored content is not compatible with <typeparamref name="T"/>.
    /// </exception>
    public T GetDirectory<T>(string name) where T : ZipperDirectory
        => (T)_content[name];

    /// <summary>
    /// Gets a contained directory identified by a GUID.
    /// </summary>
    /// <param name="id">The identifier of the directory.</param>
    /// <returns>The requested directory.</returns>
    /// <exception cref="KeyNotFoundException">
    /// No content exists under the specified identifier.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// The stored content is not a <see cref="ZipperDirectory"/>.
    /// </exception>
    public ZipperDirectory GetDirectory(Guid id)
        => GetDirectory(id.ToString());

    /// <summary>
    /// Gets a contained directory by name.
    /// </summary>
    /// <param name="name">The name of the directory.</param>
    /// <returns>The requested directory.</returns>
    /// <exception cref="KeyNotFoundException">
    /// No content exists under the specified name.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// The stored content is not a <see cref="ZipperDirectory"/>.
    /// </exception>
    public ZipperDirectory GetDirectory(string name)
        => (ZipperDirectory)_content[name];

    /// <summary>
    /// Gets a contained file identified by a GUID.
    /// </summary>
    /// <param name="id">The identifier of the file.</param>
    /// <returns>The requested file.</returns>
    /// <exception cref="KeyNotFoundException">
    /// No content exists under the specified identifier.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// The stored content is not a <see cref="ZipperFile"/>.
    /// </exception>
    public ZipperFile GetFile(Guid id)
        => GetFile(id.ToString());

    /// <summary>
    /// Gets a contained file by name.
    /// </summary>
    /// <param name="name">The name of the file.</param>
    /// <returns>The requested file.</returns>
    /// <exception cref="KeyNotFoundException">
    /// No content exists under the specified name.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// The stored content is not a <see cref="ZipperFile"/>.
    /// </exception>
    public ZipperFile GetFile(string name)
        => (ZipperFile)_content[name];

    /// <summary>
    /// Gets a contained file of the specified type by GUID.
    /// </summary>
    /// <typeparam name="T">The expected file type.</typeparam>
    /// <param name="id">The identifier of the file.</param>
    /// <returns>The file cast to <typeparamref name="T"/>.</returns>
    /// <exception cref="KeyNotFoundException">
    /// No content exists under the specified identifier.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// The stored content is not compatible with <typeparamref name="T"/>.
    /// </exception>
    public T GetFile<T>(Guid id) where T : ZipperFile
        => (T)GetFile(id.ToString());

    /// <summary>
    /// Gets a contained file of the specified type by name.
    /// </summary>
    /// <typeparam name="T">The expected file type.</typeparam>
    /// <param name="name">The name of the file.</param>
    /// <returns>The file cast to <typeparamref name="T"/>.</returns>
    /// <exception cref="KeyNotFoundException">
    /// No content exists under the specified name.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// The stored content is not compatible with <typeparamref name="T"/>.
    /// </exception>
    public T GetFile<T>(string name) where T : ZipperFile
        => (T)_content[name];

    /// <summary>
    /// Determines whether content exists under the specified GUID.
    /// </summary>
    /// <param name="id">The identifier to look for.</param>
    /// <returns>
    /// <see langword="true"/> if an entry exists with the identifier;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool HasFile(Guid id)
        => HasFile(id.ToString());

    /// <summary>
    /// Determines whether content exists under the specified name.
    /// </summary>
    /// <param name="name">The name to look for.</param>
    /// <returns>
    /// <see langword="true"/> if an entry exists with the name;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Checks for any registered content, not exclusively
    /// <see cref="ZipperFile"/> instances.
    /// </remarks>
    public bool HasFile(string name) => _content.ContainsKey(name);

    /// <summary>
    /// Attempts to retrieve a contained directory of the specified type.
    /// </summary>
    /// <typeparam name="T">The expected directory type.</typeparam>
    /// <param name="name">The name of the directory.</param>
    /// <param name="directory">
    /// When successful, receives the requested directory; otherwise, receives <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if compatible directory content was found;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetDirectory<T>(string name, out T? directory)
        where T : ZipperDirectory
        => TryGetContent(name, out directory);

    /// <summary>
    /// Attempts to retrieve a contained file of the specified type.
    /// </summary>
    /// <typeparam name="T">The expected file type.</typeparam>
    /// <param name="name">The name of the file.</param>
    /// <param name="file">
    /// When successful, receives the requested file; otherwise, receives <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if compatible file content was found; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetFile<T>(string name, out T? file)
        where T : ZipperFile
        => TryGetContent(name, out file);

    /// <summary>
    /// Attempts to retrieve content of the specified type by name.
    /// </summary>
    /// <typeparam name="T">
    /// The expected content type, implementing <see cref="IZipperContent"/>.
    /// </typeparam>
    /// <param name="name">The name of the content to retrieve.</param>
    /// <param name="result">
    /// When successful, receives the matching content;
    /// otherwise, receives <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the entry exists and is compatible
    /// with <typeparamref name="T"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Returns <see langword="false"/> when an entry exists but
    /// cannot be assigned to <typeparamref name="T"/>.
    /// </remarks>
    public bool TryGetContent<T>(string name, out T? result)
        where T : IZipperContent
    {
        if (_content.TryGetValue(name, out IZipperContent? foundResult) &&
            foundResult is T foundExact)
        {
            result = foundExact;
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Creates or retrieves a subdirectory and attempts to wrap it.
    /// </summary>
    /// <typeparam name="T">The type of directory to create or retrieve.</typeparam>
    /// <typeparam name="Q">The type of directory wrapper to create.</typeparam>
    /// <param name="name">The name of the subdirectory.</param>
    /// <returns>
    /// A wrapper of type <typeparamref name="Q"/> around the directory,
    /// or <see langword="null"/> if wrapping is unsuccessful.
    /// </returns>
    /// <remarks>
    /// Uses <see cref="CreateSubDirectory{T}(string)"/> to obtain the
    /// directory and then <see cref="WrapDirectory{T}(string)"/>
    /// to construct its wrapper.
    /// </remarks>
    public Q? CreateAndWrapDirectory<T, Q>(string name)
        where T : ZipperDirectory
        where Q : ZipperDirectoryWrapper
    {
        Q? wrapper = null;

        T? directory = CreateSubDirectory<T>(name);
        if (directory != null)
        {
            wrapper = WrapDirectory<Q>(directory);
        }

        return wrapper;
    }

    /// <summary>
    /// Gets an existing subdirectory of the specified type or creates one.
    /// </summary>
    /// <typeparam name="T">The type of directory to create or retrieve.</typeparam>
    /// <param name="name">The name of the subdirectory.</param>
    /// <returns>
    /// The existing compatible subdirectory, or a newly created subdirectory of type <typeparamref name="T"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// An entry with the specified name already exists but is not compatible with <typeparamref name="T"/>.
    /// </exception>
    public T CreateSubDirectory<T>(string name)
        where T : ZipperDirectory
    {
        if (TryGetDirectory(name, out T? foundDirectory) &&
            foundDirectory != null)
        {
            return foundDirectory;
        }

        T? directory = Zipper.CreateDirectory<T>(name);

        Add(directory.Name, directory);

        return directory;
    }

    /// <summary>
    /// Gets or creates a subdirectory with the specified name.
    /// </summary>
    /// <param name="name">The name of the subdirectory.</param>
    /// <returns>The existing or newly created subdirectory.</returns>
    public ZipperDirectory CreateSubDirectory(string name)
        => CreateSubDirectory<ZipperDirectory>(name);

    /// <summary>
    /// Gets or creates a hierarchy of nested subdirectories.
    /// </summary>
    /// <param name="paths">
    /// The ordered directory names forming the hierarchy, from the immediate child to the deepest descendant.
    /// </param>
    /// <param name="returnDirectory">
    /// The zero-based index of the directory to return. A value of -1 selects the deepest directory.
    /// </param>
    /// <returns>
    /// The directory at the requested index within the hierarchy.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="paths"/> contains no directory names.
    /// </exception>
    public ZipperDirectory CreateSubDirectory(
        string[] paths,
        int returnDirectory = -1)
    {
        returnDirectory = (returnDirectory == -1)
            ? paths.Length - 1
            : returnDirectory;

        if (paths.Length == 0)
            throw new ArgumentException(
                "A path part is required to create a sub directory.");

        string initialPart = paths[0];
        ZipperDirectory current = CreateSubDirectory(initialPart);
        ZipperDirectory result = current;

        for (int i = 1; i < paths.Length; i++)
        {
            string part = paths[i];
            current = current.CreateSubDirectory(part);

            if (returnDirectory == i)
                result = current;
        }

        return result;
    }

    /// <summary>
    /// Initializes the directory after its name has been assigned.
    /// </summary>
    protected virtual void Initalise()
    {

    }

    /// <summary>
    /// Prepares the directory and all its contained content for saving.
    /// </summary>
    /// <remarks>
    /// Raises <see cref="OnPreparingToSave"/> before recursively invoking
    /// <see cref="IZipperContent.PrepareToSave"/> on each contained item.
    ///
    /// Derived classes may override this method to implement custom
    /// preparation behavior.
    /// </remarks>
    public virtual void PrepareToSave()
    {
        OnPreparingToSave?.Invoke(this, EventArgs.Empty);

        foreach (var item in _content.Values)
        {
            item.PrepareToSave();
        }
    }
}