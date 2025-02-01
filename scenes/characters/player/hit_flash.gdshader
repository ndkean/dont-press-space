shader_type canvas_item;

uniform bool active = false;
uniform vec4 hit_color : source_color = vec4(1.0, 1.0, 1.0, 1.0);

void fragment() {
	vec4 previous_color = texture(TEXTURE, UV);
	vec4 white_color = vec4(
		hit_color.r + previous_color.r,
		hit_color.g + previous_color.g, 
		hit_color.b + previous_color.b, 
		previous_color.a);
		
	vec4 new_color = previous_color;
	if (active == true)
	{
		new_color = white_color;
	}
	COLOR = new_color;
}