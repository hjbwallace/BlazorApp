window.ShowAlert = (message) => {
    alert(message);
}

window.ShowConfirm = (message) => {
    return confirm(message);
}

window.ShowPrompt = (message) => {
    return prompt(message, '');
}