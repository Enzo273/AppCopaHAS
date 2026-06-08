using AppCopaHAS.Models.DTOs;
using AppCopaHAS.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppCopaHAS.ViewModels
{
    public class TabelaViewModel : BaseViewModel
    {
        JogoService _jogoService;
        public ObservableCollection<JogoDTO> Jogos { get; set; }

        public TabelaViewModel()
        {
            _jogoService = new JogoService();
            Jogos = new ObservableCollection<JogoDTO>();

            // Passo 4 – aciona a busca assim que a ViewModel é criada
            _ = ObterJogos();
        }

        // Passo 4 – método que busca a tabela de jogos na API
        public async Task ObterJogos()
        {
            try
            {
                Jogos = await _jogoService.GetJogosDTOAsync();
                OnPropertyChanged(nameof(Jogos));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
         "Erro",
         ex.Message,
         "OK"
     );
            }
        }
    }
}