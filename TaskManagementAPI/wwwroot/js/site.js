// Set up AJAX to include auth token
$.ajaxSetup({
    beforeSend: function (xhr) {
        const token = localStorage.getItem('authToken');
        if (token) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + token);
        }
    }
});

// Check authentication on page load
$(document).ready(function () {
    const token = localStorage.getItem('authToken');
    if (!token && window.location.pathname !== '/Account/Login') {
        window.location.href = '/Account/Login';
    }
});