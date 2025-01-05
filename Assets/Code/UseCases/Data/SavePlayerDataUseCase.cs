using System;
using System.Collections.Generic;

namespace UseCases.Services
{
    public class SavePlayerDataUseCase
    {
        private readonly IPlayFabService _playFabService;

        public SavePlayerDataUseCase(IPlayFabService playFabService)
        {
            _playFabService = playFabService;
        }

        public void Execute(Dictionary<string, string> data, Action onSuccess, Action<string> onFailure)
        {
            _playFabService.SavePlayerData(data, onSuccess, onFailure);
        }
    }
}
