using Godot;
using System.Collections.Generic;

namespace Isometric3DEngine
{
    /**
     * @brief This class is used to load and save user data in JSON format.
     *
     * Open Project Settings -> General -> Config, and turn on Custom User Dir with a Name, such as the game name.
     * Files will be saved in the user's home directory \AppData\Roaming\{CustomUserDirName}, which in Godot
     * is OS.GetUserDataDir() or just point to 'user://filename.json'
     *
     * Recommended structure for save files:
     * - player.json : 		 player specific data that aren't related to specific scenes
     * - {scene_name}.json : scene specific data
     */
    public class UserJsonFile
    {
        string FilePath;

        public UserJsonFile(string fileName)
        {
            string prefix = fileName.StartsWith("user://") ? "" : "user://";
            FilePath = prefix + fileName;
        }

        public string LoadAsJson()
        {
            if (!FileAccess.FileExists(FilePath))
                return null;

            var fp = FileAccess.Open(FilePath, FileAccess.ModeFlags.Read);
            string json = fp.GetAsText();
            fp.Close();

            return json;
        }

        public Godot.Collections.Dictionary LoadAsDictionary()
        {
            var savedJsonString = LoadAsJson();
            var savedJson = Json.ParseString(savedJsonString).AsGodotDictionary();
            return savedJson;
        }

        public void SaveJson(string json)
        {
            var fp = FileAccess.Open(FilePath, FileAccess.ModeFlags.Write);
            fp.StoreString(json);
            fp.Close();
        }

        public void SaveDictionary(Godot.Collections.Dictionary dictionary)
        {
            var fp = FileAccess.Open(FilePath, FileAccess.ModeFlags.Write);
            fp.StoreString(Json.Stringify(dictionary));
            fp.Close();
        }

        public void SetValue(string key, Variant value)
        {
            var savedJson = LoadAsDictionary();
            savedJson[key] = value;
            SaveDictionary(savedJson);
        }

        public Variant GetValue(string key, Variant defaultValue)
        {
            var savedJson = LoadAsDictionary();
            return savedJson.GetValueOrDefault(key, defaultValue);
        }

        public void SetVector3Value(string key, Vector3 value)
        {
            var vector3AsDictionary = new Godot.Collections.Dictionary<string, float>
            {
                { "x", value.X },
                { "y", value.Y },
                { "z", value.Z },
            };
            SetValue(key, vector3AsDictionary);
        }

        public Vector3 GetVector3Value(string key, Vector3 defaultValue)
        {
            var value = GetValue(key, defaultValue).AsGodotDictionary();

            return new Vector3(
                (float)value.GetValueOrDefault("x"),
                (float)value.GetValueOrDefault("y"),
                (float)value.GetValueOrDefault("z")
            );
        }
    }
}
