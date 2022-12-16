
(function () {
    isWindows = navigator.platform.indexOf('Win') > -1 ? true : false;

    if (isWindows && !$('body').hasClass('sidebar-mini')) {
        // if we are on windows OS we activate the perfectScrollbar function
        $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar();

        $('html').addClass('perfect-scrollbar-on');
    } else {
        $('html').addClass('perfect-scrollbar-off');
    }
})();

var breakCards = true;

var searchVisible = 0;
var transparent = true;

var transparentDemo = true;
var fixedTop = false;

var mobile_menu_visible = 0,
    mobile_menu_initialized = false,
    toggle_initialized = false,
    bootstrap_nav_initialized = false;

var seq = 0,
    delays = 80,
    durations = 500;
var seq2 = 0,
    delays2 = 80,
    durations2 = 500;


$(document).ready(function () {

    // Javascript method's body can be found in assets/js/demos.js
    demo.initDashboardPageCharts();

    demo.initVectorMap();

    //Init Datepicker
    demo.initFormExtendedDatetimepickers();

    $sidebar = $('.sidebar');

    $.material.init();

    md.initSidebarsCheck();

    if ($('body').hasClass('sidebar-mini')) {
        md.misc.sidebar_mini_active = true;
    }

    window_width = $(window).width();

    // check if there is an image set for the sidebar's background
    md.checkSidebarImage();

    md.initMinimizeSidebar();

    //    Activate bootstrap-select
    if ($(".selectpicker").length != 0) {
        $(".selectpicker").selectpicker();
    }

    //  Activate the tooltips
    $('[rel="tooltip"]').tooltip();

    //removed class label and label-color from tag span and replaced with data-color
    var tagClass = $('.tagsinput').data('color');

    $('.tagsinput').tagsinput({
        tagClass: ' tag-' + tagClass + ' '
    });

    //    Activate bootstrap-select
    $(".select").dropdown({
        "dropdownClass": "dropdown-menu",
        "optionClass": ""
    });

    $('.form-control').on("focus", function () {
        $(this).parent('.input-group').addClass("input-group-focus");
    }).on("blur", function () {
        $(this).parent(".input-group").removeClass("input-group-focus");
    });


    if (breakCards == true) {
        // We break the cards headers if there is too much stress on them :-)
        $('[data-header-animation="true"]').each(function () {
            var $fix_button = $(this)
            var $card = $(this).parent('.card');

            $card.find('.fix-broken-card').click(function () {
                console.log(this);
                var $header = $(this).parent().parent().siblings('.card-header, .card-image');

                $header.removeClass('hinge').addClass('fadeInDown');

                $card.attr('data-count', 0);

                setTimeout(function () {
                    $header.removeClass('fadeInDown animate');
                }, 480);
            });

            // $card.mouseenter(function () {
            //     var $this = $(this);
            //     hover_count = parseInt($this.attr('data-count'), 10) + 1 || 0;
            //     $this.attr("data-count", hover_count);

            //     if (hover_count >= 20) {
            //         $(this).children('.card-header, .card-image').addClass('hinge animated');
            //     }
            // });
        });
    }

    // remove class has-error for checkbox validation
    $('input[type="checkbox"][required="true"], input[type="radio"][required="true"]').on('click', function () {
        if ($(this).hasClass('error')) {
            $(this).closest('div').removeClass('has-error');
        }
    });



    //Toggle Show/Hide "Teachers Information" "Students Information"
    $('#toggleBtn').click(function () {
        $('#showToggle').toggle(500);
        var el = $(this);
        if (el.text() == el.data("text-swap")) {
            el.text(el.data("text-original"));
        } else {
            el.data("text-original", el.text());
            el.text(el.data("text-swap"));
        }
    });

    $('.datatables').DataTable({
        "pagingType": "full_numbers",
        "lengthMenu": [
            [10, 25, 50, -1],
            [10, 25, 50, "Tất cả"]
        ],
        responsive: true,
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Tìm kiếm",
        }

    });

    var table = $('.datatables').DataTable();

    // Delete a record
    table.on('click', '.remove', function (e) {
        $tr = $(this).closest('tr');
        table.row($tr).remove().draw();
        e.preventDefault();
    });

    $('.card .material-datatables label').addClass('form-group');

});

$(document).on('click', '.navbar-toggle', function () {
    $toggle = $(this);

    if (mobile_menu_visible == 1) {
        $('html').removeClass('nav-open');

        $('.close-layer').remove();
        setTimeout(function () {
            $toggle.removeClass('toggled');
        }, 400);

        mobile_menu_visible = 0;
    } else {
        setTimeout(function () {
            $toggle.addClass('toggled');
        }, 430);

        var $layer = $('<div class="close-layer"></div>');

        if ($('body').find('.main-panel').length != 0) {
            $layer.appendTo(".main-panel");

        } else if (($('body').hasClass('off-canvas-sidebar'))) {
            $layer.appendTo(".wrapper-full-page");
        }

        setTimeout(function () {
            $layer.addClass('visible');
        }, 100);

        $layer.click(function () {
            $('html').removeClass('nav-open');
            mobile_menu_visible = 0;

            $layer.removeClass('visible');

            setTimeout(function () {
                $layer.remove();
                $toggle.removeClass('toggled');

            }, 400);
        });

        $('html').addClass('nav-open');
        mobile_menu_visible = 1;

    }

});


$.fn.getCursorPosition = function () {
    var input = this.get(0);
    if (!input) return; // No (input) element found
    if ('selectionStart' in input) {
        // Standard-compliant browsers
        return input.selectionStart;
    } else if (document.selection) {
        // IE
        input.focus();
        var sel = document.selection.createRange();
        var selLen = document.selection.createRange().text.length;
        sel.moveStart('character', -input.value.length);
        return sel.text.length - selLen;
    }
};

// This function will allow to return to the proper cursor position after a paste.
$.fn.setCursorPosition = function (pos) {
    return this.each(function (index, elem) {
        if (elem.setSelectionRange) {
            elem.setSelectionRange(pos, pos);
        } else if (elem.createTextRange) {
            var range = elem.createTextRange();
            range.collapse(true);
            range.moveEnd('character', pos);
            range.moveStart('character', pos);
            range.select();
        }
    });
};

$(".nospace").bind('paste', function (event) {
    event.preventDefault();
    var clipboardData = event.originalEvent.clipboardData.getData('text/plain');
    str = clipboardData.replace(/\s/g, '');
    var currentCursorPos = $(this).getCursorPosition();
    var output = [$(this).val().slice(0, currentCursorPos), str, $(this).val().slice(currentCursorPos)].join('');
    $(this).val(output);
    $(this).setCursorPosition(currentCursorPos + str.length);
});

$(".nospace").bind('keydown', function (event) {
    return event.which !== 32;
});
$(function () {
    $(".nospace").on('keypress', function (e) {
        if (e.which == 32)
            return false;
    });
})

$().ready(function () {
    demo.checkFullPageBackgroundImage();

    setTimeout(function () {
        // after 1000 ms we add the class animated to the login/register card
        $('.card').removeClass('card-hidden');
    }, 700)
});

$(document).ready(function () {
    demo.initMaterialWizard();
    setTimeout(function () {
        $('.card.wizard-card').addClass('active');
    }, 600);
});

//validate form input
$("form").validate({

    showErrors: function (errorMap, errorList) {
        // Clean up any tooltips for valid elements
        $.each(this.validElements(), function (index, element) {
            var $element = $(element);
            $element.data("title", "") // Clear the title - there is no error associated anymore
                .removeClass('has-error')
                .tooltip("destroy");
        });

        // Create new tooltips for invalid elements
        $.each(errorList, function (index, error) {
            var $element = $(error.element);
            $element.tooltip()
                .data("title", error.message)
                .closest('div')
                .addClass('has-error')
                .tooltip(); // Create a new tooltip based on the error messsage we just set in the title

        });
    }

});

//Confirm Change Password
$("#confirm").click(function (event) {
    var current_password = $('#current_password').val();
    var new_password = $('#new_password').val();
    var confirm_password = $('#confirm_password').val();
    if (current_password != '' && new_password != '' && confirm_password != '' && (new_password == confirm_password)) {
        demo.showSwal('success-message');
    }
});

//Confirm Issue Password
$(".confirm2").click(function (event) {
    var new_password = $('.new_password2').val();
    var confirm_password = $('.confirm_password2').val();
    if (new_password != '' && confirm_password != '' && (new_password == confirm_password)) {
        demo.showSwal('success-message');
        event.preventDefault();
    } else if (new_password == '' && confirm_password == '') {
        $('.new_password2, .confirm_password2').closest('div').addClass('has-error');
        event.preventDefault();
    } else if (new_password == '') {
        $('.new_password2').closest('div').addClass('has-error');
        event.preventDefault();
    }
    else if (confirm_password == '') {
        $('.confirm_password2').closest('div').addClass('has-error');
        event.preventDefault();

    } else if (confirm_password != new_password) {
        $('.confirm_password2').closest('div').addClass('has-error');
        event.preventDefault();
    }
    else
        event.preventDefault();
});

//Validate form Issue Password
function setFormValidation(id) {
    $(id).validate({
        errorPlacement: function (error, element) {
            $(element).closest('div').addClass('has-error');
        }
    });
}

//validate email
$(document).ready(function (e) {
    jQuery.validator.addMethod("validate_email", function (value, element) {

        if (/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(value)) {
            return true;
        }
        else {
            return false;
        }
    }, "Please enter a valid Email.");

    $('#email').keyup(function () {
        $('#checkform').validate({
            rules: {
                email: {
                    validate_email: true
                },
            }
                .addClass('has-error')
        });
    });
});

// activate collapse right menu when the windows is resized
$(window).resize(function () {
    md.initSidebarsCheck();

    // reset the seq for charts drawing animations
    seq = seq2 = 0;

    setTimeout(function () {
        demo.initDashboardPageCharts();
    }, 500);
});

md = {
    misc: {
        navbar_menu_visible: 0,
        active_collapse: true,
        disabled_collapse_init: 0,
    },

    checkSidebarImage: function () {
        $sidebar = $('.sidebar');
        image_src = $sidebar.data('image');

        if (image_src !== undefined) {
            sidebar_container = '<div class="sidebar-background" style="background-image: url(' + image_src + ') "/>';
            $sidebar.append(sidebar_container);
        }
    },

    initSliders: function () {
        // Sliders for demo purpose in refine cards section
        var slider = document.getElementById('sliderRegular');

        noUiSlider.create(slider, {
            start: 40,
            connect: [true, false],
            range: {
                min: 0,
                max: 100
            }
        });

        var slider2 = document.getElementById('sliderDouble');

        noUiSlider.create(slider2, {
            start: [20, 60],
            connect: true,
            range: {
                min: 0,
                max: 100
            }
        });
    },

    initSidebarsCheck: function () {
        if ($(window).width() <= 991) {
            if ($sidebar.length != 0) {
                md.initRightMenu();
            }
        }
    },

    initMinimizeSidebar: function () {

        $('#minimizeSidebar').click(function () {
            var $btn = $(this);

            if (md.misc.sidebar_mini_active == true) {
                $('body').removeClass('sidebar-mini');
                md.misc.sidebar_mini_active = false;
            } else {
                $('body').addClass('sidebar-mini');
                md.misc.sidebar_mini_active = true;
            }

            // we simulate the window Resize so the charts will get updated in realtime.
            var simulateWindowResize = setInterval(function () {
                window.dispatchEvent(new Event('resize'));
            }, 180);

            // we stop the simulation of Window Resize after the animations are completed
            setTimeout(function () {
                clearInterval(simulateWindowResize);
            }, 1000);
        });
    },

    checkScrollForTransparentNavbar: debounce(function () {
        if ($(document).scrollTop() > 260) {
            if (transparent) {
                transparent = false;
                $('.navbar-color-on-scroll').removeClass('navbar-transparent');
            }
        } else {
            if (!transparent) {
                transparent = true;
                $('.navbar-color-on-scroll').addClass('navbar-transparent');
            }
        }
    }, 17),


    initRightMenu: debounce(function () {
        $sidebar_wrapper = $('.sidebar-wrapper');

        if (!mobile_menu_initialized) {
            $navbar = $('nav').find('.navbar-collapse').children('.navbar-nav.navbar-right');

            mobile_menu_content = '';

            nav_content = $navbar.html();

            nav_content = '<ul class="nav nav-mobile-menu">' + nav_content + '</ul>';

            navbar_form = $('nav').find('.navbar-form').get(0).outerHTML;

            $sidebar_nav = $sidebar_wrapper.find(' > .nav');

            // insert the navbar form before the sidebar list
            $nav_content = $(nav_content);
            $navbar_form = $(navbar_form);
            $nav_content.insertBefore($sidebar_nav);
            $navbar_form.insertBefore($nav_content);

            $(".sidebar-wrapper .dropdown .dropdown-menu > li > a").click(function (event) {
                event.stopPropagation();

            });

            // simulate resize so all the charts/maps will be redrawn
            window.dispatchEvent(new Event('resize'));

            mobile_menu_initialized = true;
        } else {
            if ($(window).width() > 991) {
                // reset all the additions that we made for the sidebar wrapper only if the screen is bigger than 991px
                $sidebar_wrapper.find('.navbar-form').remove();
                $sidebar_wrapper.find('.nav-mobile-menu').remove();

                mobile_menu_initialized = false;
            }
        }
    }, 200),


    // initBootstrapNavbarMenu: debounce(function(){
    //
    //     if(!bootstrap_nav_initialized){
    //         $navbar = $('nav').find('.navbar-collapse').first().clone(true);
    //
    //         nav_content = '';
    //         mobile_menu_content = '';
    //
    //         //add the content from the regular header to the mobile menu
    //         $navbar.children('ul').each(function(){
    //             content_buff = $(this).html();
    //             nav_content = nav_content + content_buff;
    //         });
    //
    //         nav_content = '<ul class="nav nav-mobile-menu">' + nav_content + '</ul>';
    //
    //         $navbar.html(nav_content);
    //         $navbar.addClass('off-canvas-sidebar');
    //
    //         // append it to the body, so it will come from the right side of the screen
    //         $('body').append($navbar);
    //
    //         $toggle = $('.navbar-toggle');
    //
    //         $navbar.find('a').removeClass('btn btn-round btn-default');
    //         $navbar.find('button').removeClass('btn-round btn-fill btn-info btn-primary btn-success btn-danger btn-warning btn-neutral');
    //         $navbar.find('button').addClass('btn-simple btn-block');
    //
    //         bootstrap_nav_initialized = true;
    //     }
    // }, 500),

    startAnimationForLineChart: function (chart) {

        chart.on('draw', function (data) {
            if (data.type === 'line' || data.type === 'area') {
                data.element.animate({
                    d: {
                        begin: 600,
                        dur: 700,
                        from: data.path.clone().scale(1, 0).translate(0, data.chartRect.height()).stringify(),
                        to: data.path.clone().stringify(),
                        easing: Chartist.Svg.Easing.easeOutQuint
                    }
                });
            } else if (data.type === 'point') {
                seq++;
                data.element.animate({
                    opacity: {
                        begin: seq * delays,
                        dur: durations,
                        from: 0,
                        to: 1,
                        easing: 'ease'
                    }
                });
            }
        });

        seq = 0;
    },
    startAnimationForBarChart: function (chart) {

        chart.on('draw', function (data) {
            if (data.type === 'bar') {
                seq2++;
                data.element.animate({
                    opacity: {
                        begin: seq2 * delays2,
                        dur: durations2,
                        from: 0,
                        to: 1,
                        easing: 'ease'
                    }
                });
            }
        });

        seq2 = 0;
    }
}

// Returns a function, that, as long as it continues to be invoked, will not
// be triggered. The function will be called after it stops being called for
// N milliseconds. If `immediate` is passed, trigger the function on the
// leading edge, instead of the trailing.

function debounce(func, wait, immediate) {
    var timeout;
    return function () {
        var context = this,
            args = arguments;
        clearTimeout(timeout);
        timeout = setTimeout(function () {
            timeout = null;
            if (!immediate) func.apply(context, args);
        }, wait);
        if (immediate && !timeout) func.apply(context, args);
    };
};


$(function () {
    $("#startingDate").on("dp.change", function (e) {
        $('#endingDate').data("DateTimePicker").minDate(e.date);
    });
    $("#endingDate").on("dp.change", function (e) {
        $('#startingDate').data("DateTimePicker").maxDate(e.date);
    });
});
