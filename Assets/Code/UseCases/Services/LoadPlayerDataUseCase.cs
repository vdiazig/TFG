using System;
using System.Collections.Generic;

namespace UseCases.Services
{
    public class LoadPlayerDataUseCase
    {
        private readonly IPlayFabService _playFabService;

        public LoadPlayerDataUseCase(IPlayFabService playFabService)
        {
            _playFabService = playFabService;
        }

        public void Execute(Action<Dictionary<string, string>> onSuccess, Action<string> onFailure)
        {
            _playFabService.LoadPlayerData(
                data =>
                {
                    onSuccess?.Invoke(data); // Llama a la acción de éxito con los datos cargados
                },
                error =>
                {
                    onFailure?.Invoke(error); // Llama a la acción de fallo con el mensaje de error
                }
            );
        }
    }
}
