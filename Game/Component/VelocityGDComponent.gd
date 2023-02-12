extends Node

class_name VelocityGDComponent

var _velocity : Vector2

func update_velocity(vector : Vector2) -> void:
	_velocity = vector

func move(node : CharacterBody2D) -> void:
	node.velocity = _velocity
	node.move_and_slide()
