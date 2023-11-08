extends Control

@onready var Screen = %Screen
@onready var game_manager = get_node("/root/GameManager")
@onready var infinite_ink_options = %InfiniteInkOptions
@onready var background_music_options = %BackgroundMusicOptions

# Called when the node enters the scene tree for the first time.
func _ready():
	Screen.visible = game_manager.is_paused

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("ui_cancel") and game_manager.can_pause:
		game_manager.set_paused(not game_manager.is_paused)
		Screen.visible = game_manager.is_paused
		
		if game_manager.is_paused:
			infinite_ink_options.select(1 if game_manager.get_ink_is_infinite() else 0)
			background_music_options.select(0 if game_manager.get_background_music_on() else 1)

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
