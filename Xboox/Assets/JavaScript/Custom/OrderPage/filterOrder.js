$("#inputGroupSelect").on('change', function () {
    let range = $(this).val()
    if (!isNaN(parseInt(range))) {
       return location.href = `/Member/Order/Time/Month/${$(this).val()}`
    }
})