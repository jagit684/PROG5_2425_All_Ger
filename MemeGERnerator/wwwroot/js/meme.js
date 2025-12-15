const CANVAS_SIZE = 400;
const CANVAS_WIDTH = CANVAS_SIZE;
const CANVAS_HEIGHT = CANVAS_SIZE;
const CANVAS_CENTER_X = Math.floor(CANVAS_WIDTH / 2);

const MAX_IMAGE_SCALE = 1;

const FONT_FAMILY = 'Impact';
const DEFAULT_FONT_SIZE_PX = 40;
const TEXT_FILL_STYLE = 'white';
const TEXT_STROKE_STYLE = 'black';
const TEXT_LINE_WIDTH = 2;

const TOP_TEXT_Y = 60;
const BOTTOM_TEXT_Y = 360; // desired vertical center point for bottom text area

const DEFAULT_DOMINANT_RGB = '255,255,255';
const DROPZONE_BORDER_ACTIVE = '#0078D7';
const DROPZONE_BORDER_DEFAULT = '#aaa';

let currentImage = null; // keep the currently loaded image to redraw with new text/font sizes

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

function setDefaultFileNameFromUrl(url) {
    const fileNameInput = document.getElementById('fileName');
    if (!fileNameInput || !url) return;
    try {
        const u = new URL(url);
        const path = u.pathname;
        const base = path.substring(path.lastIndexOf('/') + 1);
        if (!base) return;
        const nameWithoutExt = base.includes('.') ? base.substring(0, base.lastIndexOf('.')) : base;
        if (nameWithoutExt) fileNameInput.value = nameWithoutExt;
    } catch {
        // Fallback for non-absolute URL strings
        const parts = url.split('?')[0].split('#')[0];
        const base = parts.substring(parts.lastIndexOf('/') + 1);
        const nameWithoutExt = base.includes('.') ? base.substring(0, base.lastIndexOf('.')) : base;
        if (nameWithoutExt) fileNameInput.value = nameWithoutExt;
    }
}

function setDefaultFileNameFromFile(file) {
    const fileNameInput = document.getElementById('fileName');
    if (!fileNameInput || !file) return;
    const name = file.name || '';
    const nameWithoutExt = name.includes('.') ? name.substring(0, name.lastIndexOf('.')) : name;
    if (nameWithoutExt) fileNameInput.value = nameWithoutExt;
}

function drawMemeImage(img) {
    const canvas = document.getElementById('memeCanvas');
    if (!canvas) return;
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

    const topTextEl = document.getElementById('topText');
    const bottomTextEl = document.getElementById('bottomText');
    const topText = topTextEl ? topTextEl.value : '';
    const bottomText = bottomTextEl ? bottomTextEl.value : '';

    const topFontSizeEl = document.getElementById('topFontSize');
    const bottomFontSizeEl = document.getElementById('bottomFontSize');
    const topFontSize = topFontSizeEl ? parseInt(topFontSizeEl.value || DEFAULT_FONT_SIZE_PX, 10) : DEFAULT_FONT_SIZE_PX;
    const bottomFontSize = bottomFontSizeEl ? parseInt(bottomFontSizeEl.value || DEFAULT_FONT_SIZE_PX, 10) : DEFAULT_FONT_SIZE_PX;

    ctx.fillStyle = TEXT_FILL_STYLE;
    ctx.strokeStyle = TEXT_STROKE_STYLE;
    ctx.textAlign = 'center';
    ctx.lineWidth = TEXT_LINE_WIDTH;

    // Horizontal center should be the center of the drawn image, not the canvas
    const imageCenterX = x + newWidth / 2;

    // Draw top text (keep baseline positioning as before, centered horizontally to image)
    ctx.font = `${topFontSize}px ${FONT_FAMILY}`;
    ctx.fillText(topText, imageCenterX, TOP_TEXT_Y);
    ctx.strokeText(topText, imageCenterX, TOP_TEXT_Y);

    // Draw bottom text centered both horizontally to the image and vertically to the bottom area center
    ctx.font = `${bottomFontSize}px ${FONT_FAMILY}`;
    const metrics = ctx.measureText(bottomText);
    const ascent = metrics.actualBoundingBoxAscent || 0;
    const descent = metrics.actualBoundingBoxDescent || 0;
    // Compute baseline so that the text box vertical center equals BOTTOM_TEXT_Y
    const bottomBaselineY = BOTTOM_TEXT_Y + (ascent - descent) / 2;
    ctx.fillText(bottomText, imageCenterX, bottomBaselineY);
    ctx.strokeText(bottomText, imageCenterX, bottomBaselineY);
}

function generateMeme() {
    if (currentImage) {
        drawMemeImage(currentImage);
    }
}

function loadImageFromFile(file) {
    if (!file) return;
    if (!file.type.startsWith('image/')) return;
    setDefaultFileNameFromFile(file);
    const reader = new FileReader();
    reader.onload = function () {
        const img = new Image();
        img.onload = function () {
            currentImage = img;
            generateMeme();
        };
        img.src = reader.result;
    };
    reader.readAsDataURL(file);
}

function loadImageFromUrl(url) {
    if (!url) return;
    setDefaultFileNameFromUrl(url);
    const img = new Image();
    // Try to enable CORS to allow canvas export if the server permits it
    img.crossOrigin = 'anonymous';
    img.onload = function () {
        currentImage = img;
        generateMeme();
    };
    img.onerror = function () {
        alert('Kon de afbeelding niet laden van deze URL. Controleer de link of CORS-instellingen.');
    };
    img.src = url;
}

function downloadMeme() {
    const canvas = document.getElementById('memeCanvas');
    const fileNameInput = document.getElementById('fileName');
    const baseName = (fileNameInput && fileNameInput.value) ? fileNameInput.value.trim() : 'meme';
    const safeName = baseName || 'meme';
    const link = document.createElement('a');
    link.download = `${safeName}.png`;
    link.href = canvas.toDataURL();
    link.click();
}

function resetMeme() {
    const topTextEl = document.getElementById('topText');
    const bottomTextEl = document.getElementById('bottomText');
    const imageInputEl = document.getElementById('imageInput');
    const imageUrlEl = document.getElementById('imageUrl');
    const topFontSizeEl = document.getElementById('topFontSize');
    const bottomFontSizeEl = document.getElementById('bottomFontSize');
    const fileNameEl = document.getElementById('fileName');

    if (topTextEl) topTextEl.value = '';
    if (bottomTextEl) bottomTextEl.value = '';
    if (imageInputEl) imageInputEl.value = '';
    if (imageUrlEl) imageUrlEl.value = '';
    if (topFontSizeEl) topFontSizeEl.value = DEFAULT_FONT_SIZE_PX;
    if (bottomFontSizeEl) bottomFontSizeEl.value = DEFAULT_FONT_SIZE_PX;
    if (fileNameEl) fileNameEl.value = '';

    const topFontSizeValueEl = document.getElementById('topFontSizeValue');
    const bottomFontSizeValueEl = document.getElementById('bottomFontSizeValue');
    if (topFontSizeValueEl) topFontSizeValueEl.textContent = DEFAULT_FONT_SIZE_PX;
    if (bottomFontSizeValueEl) bottomFontSizeValueEl.textContent = DEFAULT_FONT_SIZE_PX;

    const canvas = document.getElementById('memeCanvas');
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    currentImage = null;
}

// Event listeners
const topTextInput = document.getElementById('topText');
if (topTextInput) topTextInput.addEventListener('input', generateMeme);

const bottomTextInput = document.getElementById('bottomText');
if (bottomTextInput) bottomTextInput.addEventListener('input', generateMeme);

const imageInput = document.getElementById('imageInput');
if (imageInput) imageInput.addEventListener('change', function () {
    loadImageFromFile(this.files[0]);
});

const dropZone = document.getElementById('dropZone');
if (dropZone) {
    dropZone.addEventListener('dragover', function (e) {
        e.preventDefault();
        this.style.borderColor = DROPZONE_BORDER_ACTIVE;
    });

    dropZone.addEventListener('dragleave', function () {
        this.style.borderColor = DROPZONE_BORDER_DEFAULT;
    });

    dropZone.addEventListener('drop', function (e) {
        e.preventDefault();
        this.style.borderColor = DROPZONE_BORDER_DEFAULT;
        const file = e.dataTransfer.files[0];
        loadImageFromFile(file);
    });
}

const imageUrlInput = document.getElementById('imageUrl');
const loadUrlBtn = document.getElementById('loadUrlBtn');
if (loadUrlBtn) {
    loadUrlBtn.addEventListener('click', function () {
        loadImageFromUrl(imageUrlInput && imageUrlInput.value);
    });
}
if (imageUrlInput) {
    imageUrlInput.addEventListener('keydown', function (e) {
        if (e.key === 'Enter') {
            e.preventDefault();
            loadImageFromUrl(imageUrlInput.value);
        }
    });
}

const topFontSizeInput = document.getElementById('topFontSize');
if (topFontSizeInput) topFontSizeInput.addEventListener('input', function () {
    const v = document.getElementById('topFontSizeValue');
    if (v) v.textContent = this.value;
    generateMeme();
});

const bottomFontSizeInput = document.getElementById('bottomFontSize');
if (bottomFontSizeInput) bottomFontSizeInput.addEventListener('input', function () {
    const v = document.getElementById('bottomFontSizeValue');
    if (v) v.textContent = this.value;
    generateMeme();
});
