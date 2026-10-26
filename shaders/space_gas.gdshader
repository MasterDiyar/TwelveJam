shader_type canvas_item;

// Текстура шума
uniform sampler2D noise_tex : repeat_enable, hint_default_white;

// Настройки масштабирования и пикселизации
uniform vec2 noise_scale = vec2(1.0, 1.0);
uniform float pixel_size : hint_range(1.0, 2048.0) = 16.0;

// Пороги (thresholds)
uniform float threshold_red : hint_range(0.0, 1.0) = 0.85;
uniform float threshold_purple : hint_range(0.0, 1.0) = 0.90;
uniform float threshold_blue : hint_range(0.0, 1.0) = 0.95;

// Цвета для отрезков
uniform vec4 color_red : source_color = vec4(1.0, 0.0, 0.0, 1.0);     
uniform vec4 color_purple : source_color = vec4(0.5, 0.0, 0.5, 1.0);  
uniform vec4 color_blue : source_color = vec4(0.0, 0.0, 1.0, 1.0);    

// Функция Ease-out (замедление ближе к концу диапазона)
float ease_out(float x, float power) {
    return 1.0 - pow(1.0 - clamp(x, 0.0, 1.0), power);
}

void fragment() {
	// Пикселизация UV-координат
	vec2 grid_uv = floor(UV * pixel_size) / pixel_size;
	
	// Базовый цвет спрайта
	vec4 base_color = texture(TEXTURE, grid_uv) * COLOR;
	
	// Выборка шума
	float noise_val = texture(noise_tex, grid_uv * noise_scale).r;
	
	vec4 final_color = vec4(0.0);
	float alpha = 0.0;
	
	// Ниже минимального порога — полная прозрачность
	if (noise_val < threshold_red) {
		alpha = 0.0;
	} 
	// От красного до фиолетового
	else if (noise_val < threshold_purple) {
		float t = (noise_val - threshold_red) / (threshold_purple - threshold_red);
		
		t = ease_out(t, 2.5);
		
		final_color = color_red;
		alpha = t * base_color.a; 
	} 
	// От фиолетового до синего (второй цвет сливается к первому)
	else if (noise_val < threshold_blue) {
		float t = (noise_val - threshold_purple) / (threshold_blue - threshold_purple);
		t = ease_out(t, 2.5);
		
		final_color = mix(color_red, color_purple, t);
		alpha = base_color.a;
	} 
	// Выше синего (третий цвет сливается ко второму или держится максимум)
	else {
		// Создадим небольшой диапазон за синим порогом (например, до 1.0), 
		// чтобы синий плавно переходил от фиолетового
		float upper_range = 1.0 - threshold_blue;
		float t = upper_range > 0.0 ? (noise_val - threshold_blue) / upper_range : 1.0;
		t = ease_out(t, 2.5);
		
		final_color = mix(color_purple, color_blue, t);
		alpha = base_color.a;
	}
	
	COLOR = vec4(final_color.rgb, alpha);
}