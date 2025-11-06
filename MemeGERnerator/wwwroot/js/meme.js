const CANVAS_SIZE = 400;
const CANVAS_WIDTH = CANVAS_SIZE;
const CANVAS_HEIGHT = CANVAS_SIZE;
const CANVAS_CENTER_X = Math.floor(CANVAS_WIDTH / 2);

const MAX_IMAGE_SCALE = 1;

const FONT_FAMILY = 'Impact';
const FONT_SIZE_PX = 40;
const FONT = `${FONT_SIZE_PX}px ${FONT_FAMILY}`;

const TEXT_FILL_STYLE = 'white';
const TEXT_STROKE_STYLE = 'black';
const TEXT_LINE_WIDTH = 2;

const TOP_TEXT_Y = 60;
const BOTTOM_TEXT_Y = 360;

const DEFAULT_DOMINANT_RGB = '255,255,255';
const DROPZONE_BORDER_ACTIVE = '#0078D7';
const DROPZONE_BORDER_DEFAULT = '#aaa';

function getDominantColor(img) {
    const canvas = document.createElement('canvas');
    const ctx = canvas.getContext('2d');
    canvas.width = img.width;
    canvas.height = img.height;
    ctx.drawImage(img, 0, 0);
    const imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
    const data = imageData.data;
    const colorCount = {};
    for (let i = 0; i < data.length; i += 4) {
        const rgb = data[i] + ',' + data[i + 1] + ',' + data[i + 2];
        colorCount[rgb] = (colorCount[rgb] || 0) + 1;
    }
    let dominantColor = DEFAULT_DOMINANT_RGB;
    let maxCount = 0;
    for (const color in colorCount) {
        if (colorCount[color] > maxCount) {
            maxCount = colorCount[color];
            dominantColor = color;
        }
    }
    return 'rgb(' + dominantColor + ')';
}

function drawMemeImage(img) {
    const canvas = document.getElementById('memeCanvas');
    const ctx = canvas.getContext('2d');
    canvas.width = CANVAS_WIDTH;
    canvas.height = CANVAS_HEIGHT;

    const dominantColor = getDominantColor(img);
    ctx.fillStyle = dominantColor;
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    const scale = Math.min(CANVAS_WIDTH / img.width, CANVAS_HEIGHT / img.height, MAX_IMAGE_SCALE);
    const newWidth = img.width * scale;
    const newHeight = img.height * scale;
    const x = (CANVAS_WIDTH - newWidth) / 2;
    const y = (CANVAS_HEIGHT - newHeight) / 2;

    ctx.drawImage(img, x, y, newWidth, newHeight);

    const topText = document.getElementById('topText').value;
    const bottomText = document.getElementById('bottomText').value;

    ctx.font = FONT;
    ctx.fillStyle = TEXT_FILL_STYLE;
    ctx.strokeStyle = TEXT_STROKE_STYLE;
    ctx.textAlign = 'center';
    ctx.lineWidth = TEXT_LINE_WIDTH;

    ctx.fillText(topText, CANVAS_CENTER_X, TOP_TEXT_Y);
    ctx.strokeText(topText, CANVAS_CENTER_X, TOP_TEXT_Y);

    ctx.fillText(bottomText, CANVAS_CENTER_X, BOTTOM_TEXT_Y);
    ctx.strokeText(bottomText, CANVAS_CENTER_X, BOTTOM_TEXT_Y);
}

function generateMeme() {
    const imageInput = document.getElementById('imageInput');
    if (imageInput.files[0]) {
        const reader = new FileReader();
        reader.onload = function () {
            const img = new Image();
            img.onload = function () {
                drawMemeImage(img);
            };
            img.src = reader.result;
        };
        reader.readAsDataURL(imageInput.files[0]);
    }
}

function downloadMeme() {
    const canvas = document.getElementById('memeCanvas');
    const link = document.createElement('a');
    link.download = 'meme.png';
    link.href = canvas.toDataURL();
    link.click();
}

function resetMeme() {
    document.getElementById('topText').value = '';
    document.getElementById('bottomText').value = '';
    document.getElementById('imageInput').value = '';
    const canvas = document.getElementById('memeCanvas');
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
}

document.getElementById('topText').addEventListener('input', generateMeme);
document.getElementById('bottomText').addEventListener('input', generateMeme);

document.getElementById('imageInput').addEventListener('change', function () {
    generateMeme();
});

document.getElementById('dropZone').addEventListener('dragover', function (e) {
    e.preventDefault();
    this.style.borderColor = DROPZONE_BORDER_ACTIVE;
});

document.getElementById('dropZone').addEventListener('dragleave', function (e) {
    this.style.borderColor = DROPZONE_BORDER_DEFAULT;
});

document.getElementById('dropZone').addEventListener('drop', function (e) {
    e.preventDefault();
    this.style.borderColor = DROPZONE_BORDER_DEFAULT;
    const file = e.dataTransfer.files[0];
    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = function () {
            const img = new Image();
            img.onload = function () {
                drawMemeImage(img);
            };
            img.src = reader.result;
        };
        reader.readAsDataURL(file);
    }
});
