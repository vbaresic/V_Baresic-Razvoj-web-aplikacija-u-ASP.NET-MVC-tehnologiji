/**
 * Date/Time Picker Module
 * Custom date and time picker with support for hr and en locales
 */

document.addEventListener('DOMContentLoaded', function() {
    const dateTimeInputs = document.querySelectorAll('.date-time-input');
    
    dateTimeInputs.forEach(input => {
        initializeDateTimePicker(input);
    });

    function initializeDateTimePicker(input) {
        const browserLang = navigator.language || navigator.userLanguage;
        const isHrLocale = browserLang.startsWith('hr');
        
        // Set up localization
        const locale = {
            hr: {
                months: ['Siječanj', 'Veljača', 'Ožujak', 'Travanj', 'Svibanj', 'Lipanj',
                         'Srpanj', 'Kolovoz', 'Rujan', 'Listopad', 'Studeni', 'Prosinac'],
                weekDays: ['Ned', 'Pon', 'Uto', 'Sri', 'Čet', 'Pet', 'Sub'],
                today: 'Danas',
                clear: 'Očisti',
                ok: 'OK',
                dateFormat: 'dd.mm.yyyy',
                timeFormat: 'HH:mm'
            },
            en: {
                months: ['January', 'February', 'March', 'April', 'May', 'June',
                         'July', 'August', 'September', 'October', 'November', 'December'],
                weekDays: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
                today: 'Today',
                clear: 'Clear',
                ok: 'OK',
                dateFormat: 'mm/dd/yyyy',
                timeFormat: 'HH:mm'
            }
        };
        
        const currentLocale = isHrLocale ? locale.hr : locale.en;
        const wrapper = document.createElement('div');
        wrapper.className = 'date-time-picker-wrapper';
        
        const inputWrapper = document.createElement('div');
        inputWrapper.className = 'date-time-input-group';
        
        // Create display input
        const displayInput = document.createElement('input');
        displayInput.type = 'text';
        displayInput.className = 'date-time-display-input';
        displayInput.placeholder = `${currentLocale.dateFormat} ${currentLocale.timeFormat}`;
        displayInput.readOnly = true;
        
        // Create picker button
        const pickerBtn = document.createElement('button');
        pickerBtn.type = 'button';
        pickerBtn.className = 'date-time-picker-btn';
        pickerBtn.innerHTML = '📅';
        pickerBtn.setAttribute('aria-label', 'Open date/time picker');
        
        // Hidden input to store actual value
        const hiddenInput = input.cloneNode(true);
        hiddenInput.className = 'date-time-hidden-input';
        hiddenInput.style.display = 'none';
        
        // Replace original input
        input.parentNode.replaceChild(wrapper, input);
        wrapper.appendChild(hiddenInput);
        
        inputWrapper.appendChild(displayInput);
        inputWrapper.appendChild(pickerBtn);
        wrapper.appendChild(inputWrapper);
        
        // Create picker modal
        const pickerModal = createPickerModal(hiddenInput, displayInput, currentLocale, isHrLocale);
        wrapper.appendChild(pickerModal);
        
        // Event listeners
        displayInput.addEventListener('click', () => {
            pickerModal.style.display = pickerModal.style.display === 'none' ? 'block' : 'none';
        });
        
        pickerBtn.addEventListener('click', (e) => {
            e.preventDefault();
            pickerModal.style.display = pickerModal.style.display === 'none' ? 'block' : 'none';
        });
        
        // Close on outside click
        document.addEventListener('click', (e) => {
            if (!wrapper.contains(e.target)) {
                pickerModal.style.display = 'none';
            }
        });
        
        // Initialize display if input has value
        if (hiddenInput.value) {
            displayInput.value = formatDateTimeForDisplay(hiddenInput.value, currentLocale, isHrLocale);
        }
    }

    function createPickerModal(hiddenInput, displayInput, locale, isHrLocale) {
        const modal = document.createElement('div');
        modal.className = 'date-time-picker-modal';
        modal.style.display = 'none';
        
        const today = new Date();
        let selectedDate = hiddenInput.value ? new Date(hiddenInput.value) : today;
        
        // Date picker
        const datePickerDiv = document.createElement('div');
        datePickerDiv.className = 'date-picker-section';
        datePickerDiv.innerHTML = `
            <div class="picker-header">
                <h3>${locale.months[selectedDate.getMonth()]} ${selectedDate.getFullYear()}</h3>
                <div class="picker-nav">
                    <button class="picker-prev" aria-label="Previous month">&lt;</button>
                    <button class="picker-next" aria-label="Next month">&gt;</button>
                </div>
            </div>
            <div class="weekdays">
                ${locale.weekDays.map(day => `<div class="weekday">${day}</div>`).join('')}
            </div>
            <div class="dates"></div>
        `;
        
        // Time picker
        const timePickerDiv = document.createElement('div');
        timePickerDiv.className = 'time-picker-section';
        const currentHour = selectedDate.getHours().toString().padStart(2, '0');
        const currentMinute = selectedDate.getMinutes().toString().padStart(2, '0');
        timePickerDiv.innerHTML = `
            <div class="time-inputs">
                <div class="time-input-group">
                    <label>Hour</label>
                    <input type="number" class="hour-input" min="0" max="23" value="${currentHour}" />
                </div>
                <div class="time-separator">:</div>
                <div class="time-input-group">
                    <label>Minute</label>
                    <input type="number" class="minute-input" min="0" max="59" value="${currentMinute}" />
                </div>
            </div>
        `;
        
        // Buttons
        const buttonsDiv = document.createElement('div');
        buttonsDiv.className = 'picker-buttons';
        const todayBtn = document.createElement('button');
        todayBtn.type = 'button';
        todayBtn.className = 'btn-today';
        todayBtn.textContent = locale.today;
        
        const clearBtn = document.createElement('button');
        clearBtn.type = 'button';
        clearBtn.className = 'btn-clear';
        clearBtn.textContent = locale.clear;
        
        const okBtn = document.createElement('button');
        okBtn.type = 'button';
        okBtn.className = 'btn-ok';
        okBtn.textContent = locale.ok;
        
        buttonsDiv.appendChild(todayBtn);
        buttonsDiv.appendChild(clearBtn);
        buttonsDiv.appendChild(okBtn);
        
        modal.appendChild(datePickerDiv);
        modal.appendChild(timePickerDiv);
        modal.appendChild(buttonsDiv);
        
        // Event handlers
        renderCalendar(datePickerDiv, selectedDate, locale);
        
        datePickerDiv.querySelector('.picker-prev').addEventListener('click', () => {
            selectedDate.setMonth(selectedDate.getMonth() - 1);
            renderCalendar(datePickerDiv, selectedDate, locale);
        });
        
        datePickerDiv.querySelector('.picker-next').addEventListener('click', () => {
            selectedDate.setMonth(selectedDate.getMonth() + 1);
            renderCalendar(datePickerDiv, selectedDate, locale);
        });
        
        todayBtn.addEventListener('click', (e) => {
            e.preventDefault();
            selectedDate = new Date();
            renderCalendar(datePickerDiv, selectedDate, locale);
            const hour = timePickerDiv.querySelector('.hour-input');
            const minute = timePickerDiv.querySelector('.minute-input');
            hour.value = selectedDate.getHours().toString().padStart(2, '0');
            minute.value = selectedDate.getMinutes().toString().padStart(2, '0');
        });
        
        clearBtn.addEventListener('click', (e) => {
            e.preventDefault();
            hiddenInput.value = '';
            displayInput.value = '';
            modal.style.display = 'none';
        });
        
        okBtn.addEventListener('click', (e) => {
            e.preventDefault();
            const hour = timePickerDiv.querySelector('.hour-input').value.padStart(2, '0');
            const minute = timePickerDiv.querySelector('.minute-input').value.padStart(2, '0');
            
            selectedDate.setHours(hour, minute, 0);
            
            // Format for HTML5 datetime-local input
            const isoString = selectedDate.toISOString().slice(0, 16);
            hiddenInput.value = isoString;
            displayInput.value = formatDateTimeForDisplay(isoString, locale, isHrLocale);
            
            modal.style.display = 'none';
        });
        
        // Date click handler
        datePickerDiv.addEventListener('click', (e) => {
            if (e.target.classList.contains('date-day')) {
                document.querySelectorAll('.date-day.selected').forEach(el => el.classList.remove('selected'));
                e.target.classList.add('selected');
                selectedDate.setDate(parseInt(e.target.textContent));
            }
        });
        
        return modal;
    }

    function renderCalendar(container, date, locale) {
        const year = date.getFullYear();
        const month = date.getMonth();
        const firstDay = new Date(year, month, 1);
        const lastDay = new Date(year, month + 1, 0);
        const startDate = new Date(firstDay);
        startDate.setDate(startDate.getDate() - firstDay.getDay());
        
        const datesContainer = container.querySelector('.dates');
        datesContainer.innerHTML = '';
        
        let currentDate = new Date(startDate);
        while (currentDate <= lastDay) {
            const dayDiv = document.createElement('div');
            dayDiv.className = 'date-day';
            dayDiv.textContent = currentDate.getDate();
            
            if (currentDate.getMonth() !== month) {
                dayDiv.classList.add('other-month');
            } else if (currentDate.toDateString() === new Date().toDateString()) {
                dayDiv.classList.add('today');
            }
            
            if (currentDate.getDate() === date.getDate() && currentDate.getMonth() === month) {
                dayDiv.classList.add('selected');
            }
            
            datesContainer.appendChild(dayDiv);
            currentDate.setDate(currentDate.getDate() + 1);
        }
    }

    function formatDateTimeForDisplay(isoString, locale, isHrLocale) {
        const date = new Date(isoString);
        const day = date.getDate().toString().padStart(2, '0');
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const year = date.getFullYear();
        const hour = date.getHours().toString().padStart(2, '0');
        const minute = date.getMinutes().toString().padStart(2, '0');
        
        if (isHrLocale) {
            return `${day}.${month}.${year} ${hour}:${minute}`;
        } else {
            return `${month}/${day}/${year} ${hour}:${minute}`;
        }
    }
});
