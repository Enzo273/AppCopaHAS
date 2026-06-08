using AppCopaHAS.Models;
using AppCopaHAS.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppCopaHAS.ViewModels.Jogos
{
    public class JogoViewModel : BaseViewModel
    {
        private EstadioService _estadioService;
        private SelecaoService _selecaoService;
        private JogoService _jogoService;

        public ObservableCollection<Estadio> Estadios { get; set; }
        public ObservableCollection<Selecao> Selecoes { get; set; }
        public ObservableCollection<Jogo> Jogos { get; set; }

        // Passo 10 – ICommand vinculado ao botão Salvar no layout
        public ICommand SalvarCommand { get; set; }

        public JogoViewModel()
        {
            _estadioService = new EstadioService();
            _selecaoService = new SelecaoService();
            _jogoService = new JogoService();

            Estadios = new ObservableCollection<Estadio>();
            Selecoes = new ObservableCollection<Selecao>();
            Jogos = new ObservableCollection<Jogo>();

            _ = ObterEstadios();
            _ = ObterSelecoes();

            // Passo 10 – vincula o Command ao método de salvamento
            SalvarCommand = new Command(async () => { await SalvarResultado(); });
        }

        // ──────────────────────────────────────────────
        // Passo 3 – Estádio selecionado
        // ──────────────────────────────────────────────
        private Estadio estadioSelecionado;
        public Estadio EstadioSelecionado
        {
            get => estadioSelecionado;
            set
            {
                if (value != null)
                {
                    estadioSelecionado = value;
                    OnPropertyChanged();
                }
            }
        }

        // ──────────────────────────────────────────────
        // Passo 4 – Data selecionada
        // ──────────────────────────────────────────────
        private DateTime _dataSelecionada = DateTime.Today;
        public DateTime DataSelecionada
        {
            get => _dataSelecionada;
            set
            {
                if (_dataSelecionada != value)
                {
                    _dataSelecionada = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DataHora));
                    // Erro em DataHora vai desaparecer quando criarmos a propriedade com este nome
                }
            }
        }

        // ──────────────────────────────────────────────
        // Passo 5 – Hora selecionada
        // ──────────────────────────────────────────────
        private TimeSpan _horaSelecionada = DateTime.Now.TimeOfDay;
        public TimeSpan HoraSelecionada
        {
            get => _horaSelecionada;
            set
            {
                if (_horaSelecionada != value)
                {
                    _horaSelecionada = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DataHora));
                    // Erro em DataHora vai desaparecer quando criarmos a propriedade com este nome
                }
            }
        }

        // ──────────────────────────────────────────────
        // Passo 6 – DataHora (somente get: combina data + hora)
        // ──────────────────────────────────────────────
        public DateTime DataHora
        {
            get => DataSelecionada.Date + HoraSelecionada;
        }

        // ──────────────────────────────────────────────
        // Passo 7 – Seleção 1 (mandante) e Seleção 2 (visitante)
        // ──────────────────────────────────────────────
        private Selecao selecao1; // Seleção Mandante
        public Selecao Selecao1
        {
            get => selecao1;
            set
            {
                if (value != null)
                {
                    selecao1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private Selecao selecao2; // Seleção Visitante
        public Selecao Selecao2
        {
            get => selecao2;
            set
            {
                if (value != null)
                {
                    selecao2 = value;
                    OnPropertyChanged();
                }
            }
        }

        // ──────────────────────────────────────────────
        // Passo 8 – Gols da seleção 1 e da seleção 2
        // ──────────────────────────────────────────────
        private int golsSelecao1 = 0;
        public int GolsSelecao1
        {
            get => golsSelecao1;
            set
            {
                if (value != 0)
                {
                    golsSelecao1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private int golsSelecao2 = 0;
        public int GolsSelecao2
        {
            get => golsSelecao2;
            set
            {
                if (value != 0)
                {
                    golsSelecao2 = value;
                    OnPropertyChanged();
                }
            }
        }

        // ──────────────────────────────────────────────
        // Passo 9 – Método SalvarResultado
        // ──────────────────────────────────────────────
        public async Task SalvarResultado()
        {
            try
            {
                Jogo j = new Jogo();
                j.EstadioId = estadioSelecionado.Id;
                j.DataHora = DataHora;

                JogoSelecao mandante = new JogoSelecao();
                mandante.SelecaoId = selecao1.Id;
                mandante.Gols = golsSelecao1;

                JogoSelecao visitante = new JogoSelecao();
                visitante.SelecaoId = selecao2.Id;
                visitante.Gols = golsSelecao2;

                j.JogoSelecoes.Add(mandante);
                j.JogoSelecoes.Add(visitante);

                if (j.Id == 0)
                {
                    Jogo jogoRetorno = await _jogoService.PostJogoAsync(j);

                    await Shell.Current.DisplayAlert(
    "Mensagem",
    "Dados salvos com sucesso!",
    "Ok"
);
                }

                // Passo 9 (parte final) – redireciona para a Tabela sem gerar seta de retorno
                await Shell.Current.GoToAsync("//tabela");
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

        // ──────────────────────────────────────────────
        // Métodos de carregamento de dados
        // ──────────────────────────────────────────────
        public async Task ObterEstadios()
        {
            try
            {
                Estadios = await _estadioService.GetEstadiosAsync();
                OnPropertyChanged(nameof(Estadios));
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

        public async Task ObterSelecoes()
        {
            try
            {
                Selecoes = await _selecaoService.GetSelecoesAsync();
                OnPropertyChanged(nameof(Selecoes));
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