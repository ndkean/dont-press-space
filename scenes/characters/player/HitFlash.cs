using Godot;
using System;
using System.Diagnostics;

public partial class HitFlash : Node
{
	[Export] private NodePath _spritePath = "../";
	[Export] private Color _hitColor = Colors.Red;
	[Export] private Color _flashColor = Colors.White;

	[Export] private float _initialFlashDuration;
	[Export] private float _invincibilityFlashDuration;

	private bool _flashOn;
	private bool _invincibleState;

	private float _initialFlashTimer;
	private float _invincibilityFlashTimer;

	private Color _defaultModulate;

	private Sprite2D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>(_spritePath);
		Debug.Assert(_sprite != null, "Sprite must not be null");

		_defaultModulate = _sprite.Modulate;
	}

	public override void _Process(double delta)
	{
		if (_initialFlashTimer > 0)
		{
			_initialFlashTimer -= (float)delta;
			if (_initialFlashTimer <= 0)
			{
				SetActive(false);
				SetHitColor(_flashColor);
				_invincibleState = true;
				_invincibilityFlashTimer = _invincibilityFlashDuration;
			}
		}

		if (_invincibilityFlashTimer > 0 && _invincibleState)
		{
			_invincibilityFlashTimer -= (float)delta;

			if (_invincibilityFlashTimer <= 0)
			{
				SetActive(_flashOn);
				_flashOn = !_flashOn;
				_invincibilityFlashTimer = _invincibilityFlashDuration;
			}
		}
	}

	private void SetActive(bool active) => (_sprite.Material as ShaderMaterial)?.SetShaderParameter("active", active);
	private void SetHitColor(Color color) => (_sprite.Material as ShaderMaterial)?.SetShaderParameter("hit_color", color);

	public void OnHit(Hurtbox _)
	{
		_initialFlashTimer = _initialFlashDuration;
		_invincibleState = false;
		SetActive(true);
		SetHitColor(_hitColor);
	}

	public void OnInvincibilityStateChanged(bool invincible)
	{
		if (invincible) return;
		
		SetActive(false);
		_invincibleState = false;
	}

	public override void _ExitTree()
	{
		SetActive(false);
	}
}
