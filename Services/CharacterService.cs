using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using PruebaRickAndMorty.Models;

namespace PruebaRickAndMorty.Services
{
    public class CharacterService
    {
        private readonly HttpClient _httpClient;
        private List<CharacterModel> _localCharacters; // Lista local para el CRUD

        public CharacterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _localCharacters = new List<CharacterModel>(); // Inicializar la lista local
        }

        // Obtener personajes desde la API y guardarlos localmente
        public async Task<List<CharacterModel>> GetCharacters()
        {
            if (!_localCharacters.Any())
            {
                var response = await _httpClient.GetFromJsonAsync<RickAndMortyResponse>("https://rickandmortyapi.com/api/character");
                _localCharacters = response.Results;
            }
            return _localCharacters;
        }

        // Obtener un personaje por su ID desde la lista local
        public async Task<CharacterModel> GetCharacter(int id)
        {
            return _localCharacters.FirstOrDefault(c => c.Id == id);
        }

        public async Task<string> GetDefaultImage()
        {
            var character = await GetCharacter(19); // Obtener el personaje con ID 19
            return character?.Image; // Devuelve la imagen o null si no se encuentra
        }

        // Crear un personaje localmente
        public async Task CreateCharacter(CharacterModel character)
        {
            character.Id = _localCharacters.Max(c => c.Id) + 1; // Asignar un nuevo ID
            _localCharacters.Add(character); // Agregar a la lista local
        }

        // Actualizar un personaje localmente
        public async Task UpdateCharacter(CharacterModel character)
        {
            var existingCharacter = _localCharacters.FirstOrDefault(c => c.Id == character.Id);
            if (existingCharacter != null)
            {
                // Actualizar las propiedades del personaje
                existingCharacter.Name = character.Name;
                existingCharacter.Status = character.Status;
                existingCharacter.Species = character.Species;
                existingCharacter.Gender = character.Gender;
            }
        }

        // Eliminar un personaje localmente
        public async Task DeleteCharacter(int id)
        {
            var character = _localCharacters.FirstOrDefault(c => c.Id == id);
            if (character != null)
            {
                _localCharacters.Remove(character);
            }
        }
    }

    // Clase para la respuesta de la API
    public class RickAndMortyResponse
    {
        public List<CharacterModel> Results { get; set; }
    }
}
