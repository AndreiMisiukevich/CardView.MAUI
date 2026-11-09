using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace PanCardViewSample.ViewModels
{
	public sealed class SceneSampleViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private int _currentIndex;
		private int _ImageCount = 500;

		public SceneSampleViewModel()
		{
			Items = new ObservableCollection<CardSampleItem>
			{
				new CardSampleItem(CreateSource(), _ImageCount++, Colors.Red),
				new CardSampleItem(CreateSource(), _ImageCount++, Colors.Green),
				new CardSampleItem(CreateSource(), _ImageCount++, Colors.Gold),
				new CardSampleItem(CreateSource(), _ImageCount++, Colors.Silver),
				new CardSampleItem(CreateSource(), _ImageCount++, Colors.Blue)
			};

			PanPositionChangedCommand = new Command(v =>
			{
				var val = (bool)v;
				if (val)
				{
					CurrentIndex += 1;
					return;
				}

				CurrentIndex -= 1;
			});

		}

		public ICommand PanPositionChangedCommand { get; }

		public int CurrentIndex
		{
			get => _currentIndex;
			set
			{
				_currentIndex = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentIndex)));
			}
		}

		public ObservableCollection<CardSampleItem> Items { get; }

		private string CreateSource()
		{
			return $"https://picsum.photos/500/500?image={_ImageCount}";
		}
	}
}
