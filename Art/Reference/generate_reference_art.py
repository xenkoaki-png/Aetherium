from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


OUTPUT_DIR = Path(__file__).parent
SCALE = 3


def scaled(value):
    return int(value * SCALE)


def load_font(size):
    candidates = (
        "/usr/share/fonts/truetype/dejavu/DejaVuSans-Bold.ttf",
        "/usr/share/fonts/truetype/liberation2/LiberationSans-Bold.ttf",
    )
    for candidate in candidates:
        try:
            return ImageFont.truetype(candidate, size)
        except OSError:
            continue
    return ImageFont.load_default()


def polygon(draw, points, fill, outline=None, width=1):
    points = [(scaled(x), scaled(y)) for x, y in points]
    draw.polygon(points, fill=fill)
    if outline:
        draw.line(points + [points[0]], fill=outline, width=scaled(width), joint="curve")


def ellipse(draw, box, fill, outline=None, width=1):
    box = tuple(scaled(value) for value in box)
    draw.ellipse(box, fill=fill, outline=outline, width=scaled(width))


def rounded(draw, box, radius, fill, outline=None, width=1):
    box = tuple(scaled(value) for value in box)
    draw.rounded_rectangle(box, radius=scaled(radius), fill=fill, outline=outline, width=scaled(width))


def line(draw, points, fill, width=1):
    draw.line([(scaled(x), scaled(y)) for x, y in points], fill=fill, width=scaled(width), joint="curve")


def make_sprite_sheet():
    cell = 256
    columns = 4
    rows = 2
    image = Image.new("RGBA", (scaled(cell * columns), scaled(cell * rows)), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)

    def fighter(cx, cy, accent, armor, pose="idle", kind="human"):
        shadow = (cx - 46, cy + 74, cx + 46, cy + 91)
        ellipse(draw, shadow, (5, 9, 17, 100))
        if pose == "attack":
            polygon(draw, [(cx + 34, cy - 1), (cx + 78, cy - 34), (cx + 88, cy - 28), (cx + 53, cy + 18)], (110, 228, 243, 145))
            line(draw, [(cx + 42, cy - 12), (cx + 82, cy - 31)], (228, 255, 255, 220), 3)

        if kind == "brute":
            polygon(draw, [(cx - 53, cy - 15), (cx - 47, cy - 55), (cx - 25, cy - 74), (cx + 30, cy - 74), (cx + 53, cy - 40), (cx + 60, cy + 28), (cx + 34, cy + 72), (cx - 36, cy + 70), (cx - 60, cy + 28)], armor, (21, 28, 40, 255), 3)
            polygon(draw, [(cx - 43, cy - 42), (cx - 70, cy - 68), (cx - 34, cy - 62)], accent)
            polygon(draw, [(cx + 42, cy - 42), (cx + 70, cy - 68), (cx + 34, cy - 62)], accent)
            ellipse(draw, (cx - 22, cy - 42, cx + 24, cy - 6), (19, 26, 40, 255))
            line(draw, [(cx - 17, cy - 25), (cx + 17, cy - 25)], (255, 92, 83, 255), 6)
            polygon(draw, [(cx - 37, cy + 4), (cx - 77, cy + 19), (cx - 69, cy + 48), (cx - 35, cy + 38)], accent)
            polygon(draw, [(cx + 37, cy + 4), (cx + 77, cy + 19), (cx + 69, cy + 48), (cx + 35, cy + 38)], accent)
            for offset in (-28, 0, 28):
                line(draw, [(cx + offset, cy - 60), (cx + offset + 8, cy - 48)], (247, 214, 142, 230), 3)
            return

        if kind == "drone":
            polygon(draw, [(cx - 56, cy - 5), (cx - 76, cy - 31), (cx - 49, cy - 50), (cx - 26, cy - 39)], armor, (21, 28, 40, 255), 3)
            polygon(draw, [(cx + 56, cy - 5), (cx + 76, cy - 31), (cx + 49, cy - 50), (cx + 26, cy - 39)], armor, (21, 28, 40, 255), 3)
            ellipse(draw, (cx - 48, cy - 45, cx + 48, cy + 49), armor, (21, 28, 40, 255), 4)
            ellipse(draw, (cx - 26, cy - 18, cx + 26, cy + 20), (14, 28, 42, 255))
            line(draw, [(cx - 17, cy + 1), (cx + 17, cy + 1)], accent, 7)
            polygon(draw, [(cx - 23, cy + 48), (cx - 10, cy + 78), (cx + 10, cy + 78), (cx + 23, cy + 48)], (101, 218, 237, 130))
            for sign in (-1, 1):
                line(draw, [(cx + sign * 36, cy - 14), (cx + sign * 69, cy - 4)], (204, 238, 246, 255), 3)
                ellipse(draw, (cx + sign * 66 - 4, cy - 8, cx + sign * 66 + 4, cy), accent)
            return

        if kind == "crawler":
            polygon(draw, [(cx - 50, cy + 20), (cx - 37, cy - 29), (cx + 5, cy - 50), (cx + 48, cy - 20), (cx + 40, cy + 26), (cx + 5, cy + 48), (cx - 34, cy + 41)], armor, (21, 28, 40, 255), 3)
            for side in (-1, 1):
                for index in range(3):
                    anchor_x = cx + side * (34 - index * 6)
                    anchor_y = cy + index * 17 - 7
                    foot_x = cx + side * (66 + index * 4)
                    line(draw, [(anchor_x, anchor_y), (foot_x, anchor_y + 15), (foot_x + side * 8, anchor_y + 31)], armor, 7)
                    line(draw, [(anchor_x, anchor_y), (foot_x, anchor_y + 15), (foot_x + side * 8, anchor_y + 31)], (20, 32, 46, 255), 2)
            ellipse(draw, (cx - 17, cy - 17, cx + 24, cy + 15), (16, 29, 42, 255))
            ellipse(draw, (cx + 1, cy - 8, cx + 20, cy + 2), accent)
            return

        # Compact armored explorer silhouette.
        if pose == "attack":
            line(draw, [(cx - 15, cy + 36), (cx - 26, cy + 75), (cx - 47, cy + 78)], armor, 13)
            line(draw, [(cx + 14, cy + 33), (cx + 43, cy + 63), (cx + 58, cy + 61)], armor, 13)
            line(draw, [(cx + 17, cy - 21), (cx + 47, cy - 4), (cx + 64, cy - 27)], accent, 12)
            line(draw, [(cx - 16, cy - 20), (cx - 39, cy + 3), (cx - 27, cy + 28)], armor, 12)
        else:
            line(draw, [(cx - 14, cy + 35), (cx - 25, cy + 70), (cx - 43, cy + 76)], armor, 13)
            line(draw, [(cx + 14, cy + 35), (cx + 26, cy + 70), (cx + 43, cy + 76)], armor, 13)
            line(draw, [(cx - 18, cy - 18), (cx - 39, cy + 9), (cx - 35, cy + 30)], armor, 12)
            line(draw, [(cx + 18, cy - 18), (cx + 37, cy + 5), (cx + 34, cy + 27)], armor, 12)
        rounded(draw, (cx - 28, cy - 39, cx + 28, cy + 43), 15, armor, (20, 31, 45, 255), 3)
        polygon(draw, [(cx - 26, cy - 28), (cx - 19, cy - 47), (cx + 14, cy - 47), (cx + 28, cy - 28), (cx + 19, cy - 5), (cx - 21, cy - 5)], accent, (29, 42, 55, 255), 2)
        ellipse(draw, (cx - 15, cy - 35, cx + 17, cy - 16), (18, 30, 43, 255))
        line(draw, [(cx - 10, cy - 26), (cx + 12, cy - 26)], (153, 243, 242, 255), 3)
        polygon(draw, [(cx - 31, cy - 9), (cx - 20, cy + 16), (cx - 16, cy + 36), (cx - 31, cy + 40)], accent)
        polygon(draw, [(cx + 31, cy - 9), (cx + 20, cy + 16), (cx + 16, cy + 36), (cx + 31, cy + 40)], accent)
        line(draw, [(cx - 18, cy + 8), (cx + 18, cy + 8)], (214, 235, 239, 230), 2)
        line(draw, [(cx - 15, cy + 27), (cx + 15, cy + 27)], (12, 24, 36, 255), 3)
        # Small shoulder plates and luminous suit seams survive downscaling.
        polygon(draw, [(cx - 28, cy - 11), (cx - 40, cy - 17), (cx - 39, cy + 1), (cx - 24, cy + 8)], armor, (26, 38, 50, 255), 2)
        polygon(draw, [(cx + 28, cy - 11), (cx + 40, cy - 17), (cx + 39, cy + 1), (cx + 24, cy + 8)], armor, (26, 38, 50, 255), 2)

    specs = [
        (0, 0, "player_idle", "human", "idle", (65, 230, 202), (49, 73, 91)),
        (1, 0, "player_attack", "human", "attack", (255, 218, 112), (56, 70, 89)),
        (2, 0, "drone_idle", "drone", "idle", (80, 212, 255), (45, 80, 107)),
        (3, 0, "drone_attack", "drone", "attack", (255, 116, 128), (54, 68, 87)),
        (0, 1, "brute_idle", "brute", "idle", (255, 112, 103), (88, 55, 58)),
        (1, 1, "brute_attack", "brute", "attack", (255, 192, 106), (92, 54, 49)),
        (2, 1, "crawler_idle", "crawler", "idle", (180, 236, 112), (63, 91, 62)),
        (3, 1, "crawler_attack", "crawler", "attack", (243, 111, 157), (73, 60, 83)),
    ]
    for col, row, _, kind, pose, accent, armor in specs:
        fighter(col * cell + cell // 2, row * cell + 120, accent, armor, pose, kind)

    image = image.resize((cell * columns, cell * rows), Image.Resampling.LANCZOS)
    image.save(OUTPUT_DIR / "aetherium_combat_sprite_sheet.png")
    metadata = "Sprite sheet: aetherium_combat_sprite_sheet.png\nCanvas: 1024 x 512 px, RGBA\nGrid: 4 columns x 2 rows, 256 x 256 px cells\nPivot: bottom-center for every cell\n"
    metadata += "\n".join(f"{name}: x={col * cell}, y={(rows - row - 1) * cell}, w={cell}, h={cell}" for col, row, name, *_ in specs)
    (OUTPUT_DIR / "aetherium_combat_sprite_sheet.txt").write_text(metadata + "\n", encoding="utf-8")


def draw_iso_tile(draw, x, y, width, height, top, left, right, outline=(20, 27, 39, 255)):
    top_points = [(x, y - height // 2), (x + width // 2, y), (x, y + height // 2), (x - width // 2, y)]
    polygon(draw, top_points, top, outline, 2)
    polygon(draw, [top_points[3], top_points[2], (x, y + height // 2 + 23), (x - width // 2, y + 23)], left, outline, 2)
    polygon(draw, [top_points[2], top_points[1], (x + width // 2, y + 23), (x, y + height // 2 + 23)], right, outline, 2)
    return top_points


def make_sector_board():
    width, height = 1536, 920
    image = Image.new("RGBA", (scaled(width), scaled(height)), (11, 16, 27, 255))
    draw = ImageDraw.Draw(image)
    font = load_font(scaled(23))
    title_font = load_font(scaled(34))
    sectors = [
        ("01 / FOUNDRY WRECK", (57, 72, 91, 255), (39, 48, 65, 255), (29, 37, 51, 255), (255, 180, 92, 255)),
        ("02 / CRYSTAL SEEP", (57, 77, 94, 255), (37, 55, 74, 255), (27, 40, 59, 255), (96, 235, 239, 255)),
        ("03 / FERROFLORA VAULT", (58, 81, 68, 255), (37, 56, 48, 255), (25, 42, 41, 255), (174, 237, 130, 255)),
    ]
    panel_width = 480
    for panel, (name, top, left, right, accent) in enumerate(sectors):
        ox = 28 + panel * 505
        oy = 112
        rounded(draw, (ox, 90, ox + panel_width, 858), 18, (17, 24, 37, 255), (67, 83, 108, 255), 2)
        draw.text((scaled(ox + 22), scaled(oy - 45)), name, font=font, fill=(230, 238, 250, 255))
        for row in range(6):
            for col in range(7):
                x = ox + 54 + col * 57 + (row % 2) * 28
                y = oy + 95 + row * 42
                tile_top = top if (row + col) % 4 else tuple(min(255, v + 10) for v in top[:3]) + (255,)
                points = draw_iso_tile(draw, x, y, 64, 34, tile_top, left, right)
                if (row * 3 + col + panel) % 9 == 0:
                    polygon(draw, [(x - 11, y - 4), (x - 4, y - 22), (x + 1, y - 3)], accent, (18, 26, 39, 255), 1)
                    polygon(draw, [(x + 2, y - 3), (x + 13, y - 28), (x + 14, y + 1)], tuple(max(0, v - 23) for v in accent[:3]) + (255,), (18, 26, 39, 255), 1)
                if (row * 5 + col + panel) % 11 == 0:
                    line(draw, [(x - 15, y + 5), (x - 4, y), (x + 9, y + 2)], (233, 190, 130, 220), 2)
        # Sector landmark, set into the iso board.
        cx, cy = ox + 240, oy + 215
        if panel == 0:
            rounded(draw, (cx - 60, cy - 65, cx + 57, cy + 30), 8, (77, 83, 94, 255), (22, 30, 43, 255), 3)
            for stripe_y in (cy - 46, cy - 20, cy + 6):
                line(draw, [(cx - 43, stripe_y), (cx + 39, stripe_y)], accent, 5)
            for px in (cx - 43, cx + 40):
                ellipse(draw, (px - 8, cy - 70, px + 8, cy - 54), (255, 223, 157, 255))
        elif panel == 1:
            for dx, dy, tall in [(-40, 2, 75), (0, -15, 105), (42, 7, 62)]:
                polygon(draw, [(cx + dx - 20, cy + 36), (cx + dx - 13, cy + 36 - tall * 0.6), (cx + dx, cy - tall), (cx + dx + 17, cy + 36 - tall * 0.68), (cx + dx + 24, cy + 36)], accent, (25, 48, 67, 255), 2)
                line(draw, [(cx + dx, cy - tall + 9), (cx + dx + 4, cy + 16)], (220, 255, 255, 235), 2)
        else:
            rounded(draw, (cx - 57, cy - 62, cx + 57, cy + 34), 10, (57, 82, 61, 255), (21, 38, 39, 255), 3)
            for dx, dy in [(-40, -26), (-13, -45), (24, -23), (38, 1), (-27, 4)]:
                line(draw, [(cx + dx, cy + 5), (cx + dx - 10, cy - 22)], (65, 116, 76, 255), 5)
                ellipse(draw, (cx + dx - 19, cy - 43, cx + dx + 4, cy - 19), accent, (32, 70, 58, 255), 2)
                ellipse(draw, (cx + dx - 10, cy - 36, cx + dx - 4, cy - 30), (235, 255, 191, 255))
        # Distinct hazard marker and lighting pool for each sector.
        ellipse(draw, (ox + 45, oy + 380, ox + 158, oy + 416), tuple(accent[:3]) + (55,))
        polygon(draw, [(ox + 103, oy + 342), (ox + 118, oy + 371), (ox + 88, oy + 371)], (244, 224, 153, 240), (34, 39, 49, 255), 2)
        line(draw, [(ox + 103, oy + 350), (ox + 103, oy + 361)], (27, 35, 44, 255), 3)
        draw.text((scaled(ox + 22), scaled(oy + 435)), ["HEAT / SALVAGE", "LUMEN / SPORES", "TOXIN / FERROFLORA"][panel], font=font, fill=accent)
        draw.text((scaled(ox + 22), scaled(oy + 474)), ["Sooted alloy + amber warning lamps", "Wet stone + refractive crystal growth", "Rootbound hull + bioluminescent moss"][panel], font=load_font(scaled(15)), fill=(181, 198, 219, 255))

    draw.text((scaled(38), scaled(27)), "AETHERIUM / SECTOR VARIATIONS", font=title_font, fill=(238, 243, 255, 255))
    draw.text((scaled(40), scaled(67)), "Shared iso grid, distinct material language, hazards, silhouettes, and light", font=font, fill=(156, 179, 205, 255))
    image = image.resize((width, height), Image.Resampling.LANCZOS)
    image.save(OUTPUT_DIR / "aetherium_sector_variations.png")


if __name__ == "__main__":
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    make_sprite_sheet()
    make_sector_board()
    print("Generated combat sprite sheet and sector variation board in Art/Reference")