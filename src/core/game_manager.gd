extends Node

var is_paused = false
var can_pause = true
var ink_is_infinite = false
var background_music_on = true

signal game_set_paused(paused)
signal quit_game


func _ready():
	var signal_manager = get_node("/root/SignalManager")
	signal_manager.connect("SetPlayerInkIsInfinite", _update_ink_is_infinite)
	signal_manager.connect("SetBackgroundMusicOn", _update_background_music_on)


func set_paused(paused):
	if not can_pause or paused == is_paused:
		return

	is_paused = paused
	get_tree().paused = is_paused
	emit_signal("game_set_paused", paused)


func quit():
	emit_signal("quit_game")
	get_tree().quit()


func get_ink_is_infinite():
	return ink_is_infinite


func get_background_music_on():
	return background_music_on


func _update_ink_is_infinite(value):
	ink_is_infinite = value

func _update_background_music_on(value):
	background_music_on = value
