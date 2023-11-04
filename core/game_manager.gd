extends Node

var is_paused = false
var can_pause = true

signal game_set_paused(paused)
signal quit_game

func set_paused(paused):
	if not can_pause or paused == is_paused:
		return

	is_paused = paused
	get_tree().paused = is_paused
	emit_signal("game_set_paused", paused)


func quit():
	emit_signal("quit_game")
	get_tree().quit()
