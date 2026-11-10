using System.Threading.Tasks;
using Godot;

public partial class Main : Control
{
	[Export] public Texture2D[] Symbols;

	private int _credits = 100;
	private int _bet = 10;
	private TextureRect[] _reels;

	public override void _Ready()
	{
		_reels = [GetNode<TextureRect>("%Reel1"), GetNode<TextureRect>("%Reel2"), GetNode<TextureRect>("%Reel3")];
		UpdateCredits();
	}

	private async void OnSpinButtonPressed()
	{
		if (_credits < _bet)
			return;
		_credits -= _bet;
		UpdateCredits();
		GetNode<Label>("%ResultLabel").Text = "";
		GetNode<Button>("%SpinButton").Disabled = true;
		await Spin();
		Pay();
		GetNode<Button>("%SpinButton").Disabled = false;
	}

	private async Task Spin()
	{
		for (int tick = 0; tick < 30; tick++)
		{
			for (int i = 0; i < _reels.Length; i++)
			{
				if (tick < 10 + i * 10)
					_reels[i].Texture = Symbols[GD.Randi() % Symbols.Length];
			}
			await ToSignal(GetTree().CreateTimer(0.05), SceneTreeTimer.SignalName.Timeout);
		}
	}

	private void Pay()
	{
		var a = _reels[0].Texture;
		var b = _reels[1].Texture;
		var c = _reels[2].Texture;
		int win = 0;
		if (a == b && b == c)
			win = _bet * 10;
		else if (a == b || b == c || a == c)
			win = _bet;
		_credits += win;
		GetNode<Label>("%ResultLabel").Text = win > 0 ? $"You win {win}!" : "Try again";
		UpdateCredits();
	}

	private void UpdateCredits()
	{
		GetNode<Label>("%CreditsLabel").Text = $"Credits: {_credits}";
	}
}
