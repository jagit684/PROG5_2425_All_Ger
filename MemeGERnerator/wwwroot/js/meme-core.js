// Canvas configuration constants
const CANVAS_SIZE = 400;
const CANVAS_WIDTH = CANVAS_SIZE;
const CANVAS_HEIGHT = CANVAS_SIZE;
const CANVAS_CENTER_X = Math.floor(CANVAS_WIDTH / 2);

// Text rendering constants
const FONT_FAMILY = 'Impact';
const DEFAULT_FONT_SIZE_PX = 40;
const TEXT_FILL_STYLE = 'white';
const TEXT_STROKE_STYLE = 'black';
const TEXT_LINE_WIDTH = 2;
const TOP_TEXT_Y = 60;
const BOTTOM_TEXT_Y = 360; // desired vertical center point for bottom text area

// Color constants
const DEFAULT_DOMINANT_RGB = '255,255,255';
const LIGHT_GRAY_COLOR = 'rgb(240,240,240)';
const DARK_GRAY_COLOR = 'rgb(64,64,64)';

// State variables
let currentImage = null; // keep the currently loaded image to redraw with new text/font sizes
let downloadedFileNames = {}; // track downloaded file names to prevent duplicates
let dominantColor = null; // store dominant color
let secondaryColor = null; // store secondary color

/**
 * Analyzes an image and returns the dominant color and a secondary color
 * that is approximately 50% less visible than the dominant color
 * @param {HTMLImageElement} img - The image to analyze
 * @returns {Object} Object with dominant and secondary color as RGB strings
 */
function getTopTwoColors(img) {
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
    
    // Sort colors by count (descending)
    const sortedColors = Object.entries(colorCount)
        .sort((a, b) => b[1] - a[1]);
    
    if (sortedColors.length === 0) {
        return {
            dominant: 'rgb(' + DEFAULT_DOMINANT_RGB + ')',
            secondary: 'rgb(' + DEFAULT_DOMINANT_RGB + ')'
        };
    }
    
    const dominant = sortedColors[0][0];
    const dominantCount = sortedColors[0][1];
    
    // If there's only one color, return it for both
    if (sortedColors.length === 1) {
        return {
            dominant: 'rgb(' + dominant + ')',
            secondary: 'rgb(' + dominant + ')'
        };
    }
    
    // Target count is 20% of the dominant color count
    const targetCount = dominantCount * 0.8;
    
    // Start with the second most common color (skip index 0 which is dominant)
    let secondary = sortedColors[1][0];
    let minDifference = Math.abs(sortedColors[1][1] - targetCount);
    
    // Search through remaining colors to find closest to 50% target
    for (let i = 2; i < sortedColors.length; i++) {
        const currentColor = sortedColors[i][0];
        const currentCount = sortedColors[i][1];
        const difference = Math.abs(currentCount - targetCount);
        
        // Update if this color is closer to the target
        if (difference < minDifference) {
            minDifference = difference;
            secondary = currentColor;
        }
        
        // Stop searching if we're getting too far below the target (less than 25%)
        if (currentCount < targetCount * 0.5) {
            break;
        }
    }
    
    return {
        dominant: 'rgb(' + dominant + ')',
        secondary: 'rgb(' + secondary + ')'
    };
}

/**
 * Returns the selected background color based on radio button selection
 * @returns {string} RGB color string
 */
function getSelectedBackgroundColor() {
    const selectedRadio = document.querySelector('input[name="bgColor"]:checked');
    if (!selectedRadio) return dominantColor || 'rgb(' + DEFAULT_DOMINANT_RGB + ')';
    
    const value = selectedRadio.value;
    switch (value) {
        case 'dominant':
            return dominantColor || 'rgb(' + DEFAULT_DOMINANT_RGB + ')';
        case 'secondary':
            return secondaryColor || dominantColor || 'rgb(' + DEFAULT_DOMINANT_RGB + ')';
        case 'lightgray':
            return LIGHT_GRAY_COLOR;
        case 'darkgray':
            return DARK_GRAY_COLOR;
        default:
            return dominantColor || 'rgb(' + DEFAULT_DOMINANT_RGB + ')';
    }
}

/**
 * Sets default filename from URL path
 * @param {string} url - The URL to extract filename from
 */
function setDefaultFileNameFromUrl(url) {
    const fileNameInput = document.getElementById('fileName');
    if (!fileNameInput || !url) return;
    try {
        const u = new URL(url);
        const path = u.pathname;
        const base = path.substring(path.lastIndexOf('/') + 1);
        if (!base) return;
        const nameWithoutExt = base.includes('.') ? base.substring(0, base.lastIndexOf('.')) : base;
        if (nameWithoutExt) fileNameInput.value = `meme_${nameWithoutExt}`;
    } catch {
        // Fallback for non-absolute URL strings
        const parts = url.split('?')[0].split('#')[0];
        const base = parts.substring(parts.lastIndexOf('/') + 1);
        const nameWithoutExt = base.includes('.') ? base.substring(0, base.lastIndexOf('.')) : base;
        if (nameWithoutExt) fileNameInput.value = `meme_${nameWithoutExt}`;
    }
}

/**
 * Sets default filename from file object
 * @param {File} file - The file to extract name from
 */
function setDefaultFileNameFromFile(file) {
    const fileNameInput = document.getElementById('fileName');
    if (!fileNameInput || !file) return;
    const name = file.name || '';
    const nameWithoutExt = name.includes('.') ? name.substring(0, name.lastIndexOf('.')) : name;
    if (nameWithoutExt) fileNameInput.value = `meme_${nameWithoutExt}`;
}

/**
 * Draws the meme image on canvas with text overlay
 * @param {HTMLImageElement} img - The image to draw
 */
function drawMemeImage(img) {
    const canvas = document.getElementById('memeCanvas');
    if (!canvas) return;
    const ctx = canvas.getContext('2d');
    canvas.width = CANVAS_WIDTH;
    canvas.height = CANVAS_HEIGHT;

    // Get colors from image
    const colors = getTopTwoColors(img);
    dominantColor = colors.dominant;
    secondaryColor = colors.secondary;
    
    // Update ALL color preview squares (including static ones)
    updateColorSquares();
    
    // Use selected background color
    const bgColor = getSelectedBackgroundColor();
    ctx.fillStyle = bgColor;
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    // Calculate scale to fit image within canvas while preserving aspect ratio
    // Use Math.min to ensure the entire image is visible without cropping
    const scale = Math.min(CANVAS_WIDTH / img.width, CANVAS_HEIGHT / img.height);
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

/**
 * Generates/regenerates the meme with current image and settings
 */
function generateMeme() {
    if (currentImage) {
        drawMemeImage(currentImage);
    }
}

/**
 * Loads an image from a file
 * @param {File} file - The file to load
 */
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

/**
 * Loads an image from a URL
 * @param {string} url - The URL to load image from
 */
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

/**
 * Downloads the generated meme as PNG file
 */
function downloadMeme() {
    const canvas = document.getElementById('memeCanvas');
    const fileNameInput = document.getElementById('fileName');
    const inputName = (fileNameInput && fileNameInput.value) ? fileNameInput.value.trim() : 'meme';
    
    // Ensure the name starts with "meme_"
    let baseName = inputName.startsWith('meme_') ? inputName : `meme_${inputName}`;
    
    // Check if this filename has been used before
    let finalName = baseName;
    if (downloadedFileNames[baseName]) {
        // Find the next available number
        let counter = 1;
        while (downloadedFileNames[`${baseName}(${counter})`]) {
            counter++;
        }
        finalName = `${baseName}(${counter})`;
    }
    
    // Mark this filename as used
    downloadedFileNames[finalName] = true;
    
    const link = document.createElement('a');
    link.download = `${finalName}.png`;
    link.href = canvas.toDataURL();
    link.click();
}

/**
 * Resets all meme settings to defaults
 */
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

    // Reset background color radio to dominant
    const dominantRadio = document.getElementById('bgColorDominant');
    if (dominantRadio) dominantRadio.checked = true;

    const canvas = document.getElementById('memeCanvas');
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    currentImage = null;
    dominantColor = null;
    secondaryColor = null;
    
    // Reset all color squares
    const dominantSquare = document.getElementById('colorSquareDominant');
    const secondarySquare = document.getElementById('colorSquareSecondary');
    if (dominantSquare) dominantSquare.style.backgroundColor = '';
    if (secondarySquare) secondarySquare.style.backgroundColor = '';
}