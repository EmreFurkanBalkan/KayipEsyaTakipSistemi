// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Custom Select Implementation with Div Structure
class CustomSelect {
    constructor(selectElement) {
        this.originalSelect = selectElement;
        this.options = Array.from(selectElement.options);
        this.selectedValue = selectElement.value;
        this.selectedText = selectElement.selectedOptions[0]?.textContent || '';
        
        this.wrapper = null;
        this.searchInput = null;
        this.optionsContainer = null;
        this.hiddenInput = null;
        
        this.init();
    }
    
    init() {
        // Skip if already initialized or if it's a status select (enum)
        if (this.originalSelect.classList.contains('custom-select-initialized') || 
            this.originalSelect.name === 'Status') {
            return;
        }
        
        this.createCustomSelect();
        this.bindEvents();
        
        this.originalSelect.classList.add('custom-select-initialized');
        this.originalSelect.style.display = 'none';
    }
    
    createCustomSelect() {
        // Create main wrapper div
        this.wrapper = document.createElement('div');
        this.wrapper.className = 'custom-select-wrapper';
        this.wrapper.style.cssText = `
            width: 100%;
            border: 1px solid #d1d5db;
            border-radius: 0.375rem;
            background: white;
            padding: 8px;
        `;
        
        // Create search box at the top
        this.searchInput = document.createElement('input');
        this.searchInput.type = 'text';
        this.searchInput.placeholder = 'Ara...';
        this.searchInput.style.cssText = `
            width: 100%;
            padding: 8px;
            border: 1px solid #d1d5db;
            border-radius: 0.25rem;
            font-size: 14px;
            outline: none;
            margin-bottom: 8px;
            background: #f9fafb;
        `;
        
        // Create options container
        this.optionsContainer = document.createElement('div');
        this.optionsContainer.style.cssText = `
            max-height: 200px;
            overflow-y: auto;
            border: 1px solid #e5e7eb;
            border-radius: 0.25rem;
            background: white;
        `;
        
        // Create hidden input for form submission
        this.hiddenInput = document.createElement('input');
        this.hiddenInput.type = 'hidden';
        this.hiddenInput.name = this.originalSelect.name;
        this.hiddenInput.value = this.selectedValue;
        
        // Assemble the structure
        this.wrapper.appendChild(this.searchInput);
        this.wrapper.appendChild(this.optionsContainer);
        this.wrapper.appendChild(this.hiddenInput);
        
        // Insert into DOM
        this.originalSelect.parentNode.insertBefore(this.wrapper, this.originalSelect);
        this.wrapper.appendChild(this.originalSelect);
        
        this.populateOptions();
    }
    
    populateOptions() {
        this.optionsContainer.innerHTML = '';
        
        this.options.forEach((option, index) => {
            if (option.value === '') {
                return; // Skip empty option
            }
            
            // Create option container div
            const optionDiv = document.createElement('div');
            optionDiv.style.cssText = `
                padding: 8px 12px;
                border-bottom: 1px solid #f3f4f6;
                display: flex;
                align-items: center;
                cursor: pointer;
                transition: background-color 0.2s;
            `;
            
            // Create radio input
            const radioInput = document.createElement('input');
            radioInput.type = 'radio';
            radioInput.name = `custom-select-${this.originalSelect.name}`;
            radioInput.value = option.value;
            radioInput.id = `option-${this.originalSelect.name}-${index}`;
            radioInput.style.cssText = 'margin-right: 8px;';
            
            // Check if this option is selected
            if (option.value === this.selectedValue) {
                radioInput.checked = true;
                optionDiv.style.backgroundColor = '#e0f2fe';
            }
            
            // Create label
            const label = document.createElement('label');
            label.htmlFor = radioInput.id;
            label.textContent = option.textContent;
            label.style.cssText = `
                cursor: pointer;
                flex: 1;
                margin: 0;
            `;
            
            // Add hover effects
            optionDiv.addEventListener('mouseenter', () => {
                if (!radioInput.checked) {
                    optionDiv.style.backgroundColor = '#f3f4f6';
                }
            });
            
            optionDiv.addEventListener('mouseleave', () => {
                if (!radioInput.checked) {
                    optionDiv.style.backgroundColor = 'white';
                }
            });
            
            // Handle selection
            const selectHandler = () => {
                this.selectOption(option, radioInput, optionDiv);
            };
            
            optionDiv.addEventListener('click', selectHandler);
            radioInput.addEventListener('change', selectHandler);
            
            // Assemble the option
            optionDiv.appendChild(radioInput);
            optionDiv.appendChild(label);
            this.optionsContainer.appendChild(optionDiv);
        });
    }
    
    filterOptions(searchTerm) {
        const options = this.optionsContainer.children;
        const term = searchTerm.toLowerCase();
        
        Array.from(options).forEach(optionDiv => {
            const label = optionDiv.querySelector('label');
            const text = label ? label.textContent.toLowerCase() : '';
            if (text.includes(term)) {
                optionDiv.style.display = 'flex';
            } else {
                optionDiv.style.display = 'none';
            }
        });
    }
    
    selectOption(option, radioInput, optionDiv) {
        // Update original select and hidden input
        this.originalSelect.value = option.value;
        this.hiddenInput.value = option.value;
        this.selectedValue = option.value;
        this.selectedText = option.textContent;
        
        // Update all radio buttons and styling
        Array.from(this.optionsContainer.children).forEach(div => {
            const radio = div.querySelector('input[type="radio"]');
            if (radio) {
                radio.checked = false;
                div.style.backgroundColor = 'white';
            }
        });
        
        // Set current selection
        radioInput.checked = true;
        optionDiv.style.backgroundColor = '#e0f2fe';
        
        // Trigger change event
        const event = new Event('change', { bubbles: true });
        this.originalSelect.dispatchEvent(event);
    }
    
    bindEvents() {
        // Search functionality
        this.searchInput.addEventListener('input', (e) => {
            this.filterOptions(e.target.value);
        });
        
        // Focus search input when wrapper is clicked
        this.wrapper.addEventListener('click', (e) => {
            if (e.target === this.wrapper) {
                this.searchInput.focus();
            }
        });
    }
}

// Initialize custom selects when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    // Find all select elements that should be custom selects
    const selectElements = document.querySelectorAll('select[asp-items], select[name="VehicleId"], select[name="LocationId"]');
    
    selectElements.forEach(select => {
        // Skip if it's a status select (enum) or already initialized
        if (select.name !== 'Status' && !select.classList.contains('custom-select-initialized')) {
            new CustomSelect(select);
        }
    });
});
