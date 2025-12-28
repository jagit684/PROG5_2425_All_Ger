// UI color constants
const DROPZONE_BORDER_ACTIVE = '#0078D7';
const DROPZONE_BORDER_DEFAULT = '#aaa';

/**
 * Updates all color preview squares in the UI
 */
function updateColorSquares() {
    // Update dynamic color squares (dominant and secondary from image)
    const dominantSquare = document.getElementById('colorSquareDominant');
    const secondarySquare = document.getElementById('colorSquareSecondary');
    
    if (dominantSquare && dominantColor) {
        dominantSquare.style.backgroundColor = dominantColor;
    }
    
    if (secondarySquare && secondaryColor) {
        secondarySquare.style.backgroundColor = secondaryColor;
    }
    
    // Update static color squares (light gray and dark gray)
    const lightGraySquare = document.getElementById('colorSquareLightGray');
    const darkGraySquare = document.getElementById('colorSquareDarkGray');
    
    if (lightGraySquare) {
        lightGraySquare.style.backgroundColor = LIGHT_GRAY_COLOR;
    }
    
    if (darkGraySquare) {
        darkGraySquare.style.backgroundColor = DARK_GRAY_COLOR;
    }
}

/**
 * Initializes static color squares on page load
 */
function initializeStaticColorSquares() {
    const lightGraySquare = document.getElementById('colorSquareLightGray');
    const darkGraySquare = document.getElementById('colorSquareDarkGray');
    
    if (lightGraySquare) {
        lightGraySquare.style.backgroundColor = LIGHT_GRAY_COLOR;
    }
    
    if (darkGraySquare) {
        darkGraySquare.style.backgroundColor = DARK_GRAY_COLOR;
    }
}

/**
 * Sets up all event listeners for the meme generator
 */
function setupEventListeners() {
    // Text input listeners
    const topTextInput = document.getElementById('topText');
    if (topTextInput) topTextInput.addEventListener('input', generateMeme);

    const bottomTextInput = document.getElementById('bottomText');
    if (bottomTextInput) bottomTextInput.addEventListener('input', generateMeme);

    // Image file input listener
    const imageInput = document.getElementById('imageInput');
    if (imageInput) {
        imageInput.addEventListener('change', function () {
            loadImageFromFile(this.files[0]);
        });
    }

    // Drop zone listeners
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

    // URL input listeners
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

    // Font size slider listeners
    const topFontSizeInput = document.getElementById('topFontSize');
    if (topFontSizeInput) {
        topFontSizeInput.addEventListener('input', function () {
            const v = document.getElementById('topFontSizeValue');
            if (v) v.textContent = this.value;
            generateMeme();
        });
    }

    const bottomFontSizeInput = document.getElementById('bottomFontSize');
    if (bottomFontSizeInput) {
        bottomFontSizeInput.addEventListener('input', function () {
            const v = document.getElementById('bottomFontSizeValue');
            if (v) v.textContent = this.value;
            generateMeme();
        });
    }

    // Background color radio button listeners
    const bgColorRadios = document.querySelectorAll('input[name="bgColor"]');
    bgColorRadios.forEach(radio => {
        radio.addEventListener('change', generateMeme);
    });
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', function() {
    initializeStaticColorSquares();
    setupEventListeners();
});