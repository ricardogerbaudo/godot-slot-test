extends Control

@export var symbols: Array[Texture2D]

var credits := 100
var bet := 10

@onready var reels := [%Reel1, %Reel2, %Reel3]


func _ready() -> void:
	update_credits()


func _on_spin_button_pressed() -> void:
	if credits < bet:
		return
	credits -= bet
	update_credits()
	%ResultLabel.text = ""
	%SpinButton.disabled = true
	await spin()
	pay()
	%SpinButton.disabled = false


func spin() -> void:
	for tick in 30:
		for i in reels.size():
			if tick < 10 + i * 10:
				reels[i].texture = symbols.pick_random()
		await get_tree().create_timer(0.05).timeout


func pay() -> void:
	var a = reels[0].texture
	var b = reels[1].texture
	var c = reels[2].texture
	var win := 0
	if a == b and b == c:
		win = bet * 10
	elif a == b or b == c or a == c:
		win = bet
	credits += win
	%ResultLabel.text = "You win %d!" % win if win > 0 else "Try again"
	update_credits()


func update_credits() -> void:
	%CreditsLabel.text = "Credits: %d" % credits
