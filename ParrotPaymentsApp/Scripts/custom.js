
// иконка загрузки при регистрации/авторизации
function switchSpinner() {
    var spinner = $('.spinner-login');
    spinner.toggle();
}

// иконка загрузки в панели навигации основного представления
function switchNavbarSpinner() {
    var nb_spinner = $('i.fa-cog');

    if (nb_spinner.css('display') === 'none')
        nb_spinner.css('display', 'inline-block')
    else
        nb_spinner.css('display', 'none');
}