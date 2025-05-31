// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



// Wait for the page to fully load
window.addEventListener('DOMContentLoaded', (event) => {
    // Select all Bootstrap alerts
    const alerts = document.querySelectorAll('.alert');

    // Set timeout to auto-dismiss
    alerts.forEach(alert => {
        setTimeout(() => {
            // Bootstrap 5 fade out by removing 'show' class
            alert.classList.remove('show');

            // completely remove the element after transition
            //setTimeout(() => alert.remove(), 500);
        }, 3000); // in ms
    });
});