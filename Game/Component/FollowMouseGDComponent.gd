extends Node2D

@export var velocity_component : Node
@export var speed : float = 30.0

func follow() -> void:
	var mouseDir = (get_global_mouse_position() - global_position).normalized()
	var distance = global_position.distance_to(get_global_mouse_position())
	var new_velocity = mouseDir * speed * distance;
	velocity_component.update_velocity(new_velocity)
