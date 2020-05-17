// EVECSC - Michael
// Folder.cs
// Last Cleanup: 17/05/2020 10:56
// Created: 16/05/2020 13:20

#region Directives
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using RestSharp;
#endregion

namespace EVECSC.Models
{
    public class Folder
    {
        private List<Character> _characters;

        public string Name { get; set; }
        public string FilePath { get; set; }

        public List<Character> Characters
        {
            get
            {
                if (_characters != null)
                {
                    return _characters;
                }

                _characters = GetCharacters();
                return _characters;
            }
            set => _characters = value;
        }

        public List<Character> GetCharacters()
        {
            var characters = new List<Character>();
            var characterFiles = Directory.EnumerateFiles(FilePath, "core_char_*.dat");
            var regex = new Regex("(\\d+)");

            foreach (var characterFile in characterFiles)
                if (regex.Matches(characterFile).Count > 0)
                {
                    var id = regex.Match(characterFile).Value;
                    var client = new RestClient($"https://esi.evetech.net/latest/characters/{id}/?datasource=tranquility");
                    var resp = client.Execute<Character>(new RestRequest());
                    resp.Data.ID = int.Parse(id);

                    characters.Add(resp.Data);
                }

            return characters;
        }
    }
}