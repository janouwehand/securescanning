using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SecureScan.Base.Files
{
  public class CompoundFile
  {
    private const string VALUES = nameof(VALUES);

    private class CompoundFilePartImp : CompoundFilePart { }

    public Dictionary<string, string> Values { get; } = new Dictionary<string, string>();

    public void AddPart(string filename, string contentType = "application/octet-stream")
    {
      if (string.IsNullOrWhiteSpace(filename))
      {
        throw new ArgumentException($"'{nameof(filename)}' cannot be null or whitespace.", nameof(filename));
      }

      if (string.IsNullOrWhiteSpace(contentType))
      {
        throw new ArgumentException($"'{nameof(contentType)}' cannot be null or whitespace.", nameof(contentType));
      }

      var fn = Path.GetFileName(filename);
      Parts.Add(new CompoundFilePartImp
      {
        Name = fn,
        ContentType = contentType,
        Data = File.ReadAllBytes(filename)
      });
    }

    public void AddPart(byte[] data, string name, string contentType = "application/octet-stream")
    {
      if (data == null)
      {
        throw new ArgumentNullException(nameof(data));
      }

      if (string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
      }

      if (string.IsNullOrWhiteSpace(contentType))
      {
        throw new ArgumentException($"'{nameof(contentType)}' cannot be null or whitespace.", nameof(contentType));
      }

      name = Path.GetFileName(name);

      var fn = Path.GetFileName(name);
      Parts.Add(new CompoundFilePartImp
      {
        Name = fn,
        ContentType = contentType,
        Data = data
      });
    }

    public List<CompoundFilePart> Parts { get; } = new List<CompoundFilePart>();

    public static CompoundFile FromBytes(byte[] data)
    {
      using (var ms = new MemoryStream(data))
      {
        return Load(ms);
      }
    }

    public static CompoundFile Load(Stream stream)
    {
      // Read the file and parse the parts
      var file = new CompoundFile();

      using (var reader = new BinaryReader(stream))
      {
        while (stream.Position < stream.Length)
        {
          // Read the part metadata
          var name = reader.ReadString();
          var size = reader.ReadInt32();

          // Read the part data
          var data = reader.ReadBytes(size);

          // Add the part to the list
          file.Parts.Add(new CompoundFilePartImp { Name = name, Data = data });
        }
      }

      var values = file.Parts.FirstOrDefault(p => p.Name == VALUES);
      if (values != null)
      {
        var lines = Encoding.UTF8.GetString(values.Data).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
          var parts = line.Split(new[] { '=' }, 2);
          if (parts.Length == 2)
          {
            file.Values[parts[0]] = parts[1];
          }
        }
      }

      return file;
    }

    public byte[] ToBytes()
    {
      using (var ms = new MemoryStream())
      {
        Save(ms);
        return ms.ToArray();
      }
    }

    public void Save(Stream stream)
    {
      var parts = Parts.ToList();

      var strValues = string.Join("\r\n", Values.Select(x => $"{x.Key}={x.Value}"));
      var bsValues = Encoding.UTF8.GetBytes(strValues);

      parts.Add(new CompoundFilePartImp { Name = VALUES, Data = bsValues });

      // Write the parts to the file
      using (var writer = new BinaryWriter(stream))
      {
        foreach (var part in parts)
        {
          // Write the part metadata
          writer.Write(part.Name);
          writer.Write(part.Data.Length);

          // Write the part data
          writer.Write(part.Data);
        }
      }
    }

    public CompoundFilePart GetPart(string partName) => Parts.FirstOrDefault(x => x.Name == partName);
  }
}