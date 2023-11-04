extends Control

@onready var Screen = %Screen
@onready var game_manager = get_node("/root/GameManager")

# Called when the node enters the scene tree for the first time.
func _ready():
	Screen.visible = game_manager.is_paused

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("ui_cancel"):
		if game_manager.can_pause:
			game_manager.set_paused(not game_manager.is_paused)
			Screen.visible = game_manager.is_paused

func _on_resume_button_pressed():
	game_manager.set_paused(false)
	Screen.visible = game_manager.is_paused
	
func _on_quit_button_pressed():
	game_manager.quit()

func _on_infinite_ink_options_item_selected(index):
	var signal_manager = get_node("/root/SignalManager")
	signal_manager.emit_signal("SetPlayerInkIsInfinite", index == 1)

func _on_background_music_options_item_selected(index):
	var signal_manager = get_node("/root/SignalManager")
	signal_manager.emit_signal("SetBackgroundMusicOn", index == 0)
