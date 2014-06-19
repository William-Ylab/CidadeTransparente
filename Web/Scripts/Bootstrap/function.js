// JavaScript Document

var scrollByClick;
var api;
var w;
$(function(){	
    
    
    
    w = $(window).width();
    if($(".fullwidthbanner").length) {
        $('.fullwidthbanner').revolution({
            delay:9000,
            startheight:363,
            startwidth:960,

            navigationType:"both",					//bullet, thumb, none, both		(No Thumbs In FullWidth Version !)
            navigationArrows:"verticalcentered",		//nexttobullets, verticalcentered, none
            navigationStyle:"round",				//round,square,navbar

            touchenabled:"on",						// Enable Swipe Function : on/off
            onHoverStop:"on",						// Stop Banner Timet at Hover on Slide on/off

            navOffsetHorizontal:0,
            navOffsetVertical:20,

            stopAtSlide:-1,
            stopAfterLoops:-1,

            fullWidth:"on"							// Turns On or Off the Fullwidth Image Centering in FullWidth Modus
        });
    }
	
	
	
    $("#top_menu li a").click(function(){
        var href = $(this).attr('href');
        if(href.search('_anchor') != -1){
            href = href.replace('_anchor', '_marker');
            if($(href).length){
                scrollByClick = href;
                var position = $(href).offset().top;
                $('body, html').animate({
                    scrollTop: position - 120
                    }, 500, function(){
                    scrollByClick = '';		
                });
                return false;
            }
        }
    })
	
    $("#top_menu select").change(function(){
        var href = $(this).val();
        if(href.search('_anchor') != -1){
            href = href.replace('_anchor', '_marker');
            scrollByClick = href;
            var position = $(href).offset().top;
            $('body, html').animate({
                scrollTop: position
            }, 500, function(){
                scrollByClick = '';		
            });
        }
    })
    topMenu();
    footer();
    carousels();
    infoDeviders();
	
	if($(".fancy").length){
		$("a.fancy").fancybox({        
			'padding'	: 10,
			'transitionIn'	: 'fade',
			'transitionOut'	: 'fade',
			'speedIn' : 600, 
			'speedOut' : 200, 
			'overlayShow'	: true,
			'hideOnContentClick' : true,
			'hideOnOverlayClick' : true,
			'enableEscapeButton' : true
		});
	}
	if($(".fancy-video").length){
		$("a.fancy-video").click(function() {
			$.fancybox({
				'padding' : 0,
				'autoScale' : false,
				'transitionIn' : 'fade',
				'transitionOut' : 'fade',
				'title' : this.title,
				'href' : this.href.replace(new RegExp("watch\\?v=", "i"), 'v/'),
				'type' : 'swf',
				'swf' : {
					'wmode' : 'transparent',
					'allowfullscreen' : 'true'
				}
			});
			return false;
		});	
	}
})


$(window).resize(function(){
    w = $(window).width();
    footer();
    carousels();
    infographBottom();	
    fixedHeader()
})

$(window).load(function(){
    infographBottom();
    $('body').css('visibility', 'visible');
	processBox();
    initProgressBarWithImage();
    progress_bars();
    easePieChart();
    trends();
    var hash = location.hash;
    if(hash) {
        $("#top_menu li a[href='" + hash + "']").click();
    }
    fixedHeader();
})


function fixedHeader(){
    if(w > 979){
        var h_bar = $('.notify-bar').outerHeight();
        var h_header = $('#header .inner').height();
        $('#header').css('height', h_header);

    } else {
        $('#header').removeAttr('style');
    }
     $(".notify-close").click(function(){
        var speed = 300;
        $(".notify-bar").slideUp(speed);
        if(w > 979) $('#header').animate({'height': '-=' + h_bar + 'px'}, speed);
        return false;
    }); 
}

function topMenu() {
    var markers	= $('[id*="_marker"]');
    $(window).scroll(function(){
        var top = $(window).scrollTop();
        //if(top > 15 && w > 768) {
        //    $("#header").addClass('fixed');
        //} else {
        //    $("#header").removeClass('fixed');
        //}
        markers.each(function(){
            var name_marker = '#' + $(this).attr('id');
            if(scrollByClick == name_marker || !scrollByClick) {
                var position = $(this).offset().top;
                var name_anchor = name_marker.replace('_marker', '_anchor');
                if(top >= position - $("#header .inner").height()){
                    $("#top_menu li a[href='" + name_anchor + "']").closest('li').addClass('active styler_parent_active').siblings().removeClass('active styler_parent_active');
                    $("#top_menu select option[value='" + name_anchor + "']").attr('selected', true).siblings().attr('selected', false);
                }
            }
        })
    })
	
}

map();
function map(){
    google.load('maps', '3', {
        other_params: 'sensor=false'
    });
    google.setOnLoadCallback(map_initialize);
}

function map_initialize() {
    var myLatlngCenter = new google.maps.LatLng(32.83823,-96.775347); 
    var myMarker = new google.maps.LatLng(32.83823,-96.775347);


    var mapEl = document.getElementById('map');
    if(mapEl) {
        map = new google.maps.Map( mapEl, {
            zoom: 14,
            scrollwheel: false,
            center: myLatlngCenter,
            disableDefaultUI: true,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        });
        var marker1 = new google.maps.Marker({
            position: myMarker,
            map: map,
            title:"5321 E Mockingbird Ln, Dallas, TX 75206",
            icon: '/Design/Images/Bootstrap/map_marker.png'
        });
        google.maps.event.addDomListener(window, 'resize', function() {
            map.setCenter(myLatlngCenter);
        });
    }
}

function footer(){
    var h = $("#footer").height();
    $("#footer").css('margin-top', -(h + parseInt($("#container").css('padding-bottom'))));
    $("#empty").css('height', h);
}

var carousel_enable;
function carousels(){
	
    $(".carousel ul").trigger("destroy", true);
    if(w > 768) {
        $(".carousel").each(function(){
            var ul = $(this).find('ul');
            var prev_arr = $(this).find('.left_arr');
            var next_arr = $(this).find('.right_arr');
			
            ul.carouFredSel({
                width	: "100%",
                scroll	: 3,
                align: 'left',
                prev: {
                    button: prev_arr
                },
                next: {
                    button: next_arr
                },
                auto: false
            });
			
        })	
    }
}

function infographBottom(){
    var h = 0;
    $('.infograph .desc').each(function(){
        var height = $(this).outerHeight();
        if(height > h) h = height;
    })
    if(h < 153) h = 153;
    $('#wrapper .infograph > sup').css('height', h);		
}


$(document).ready(function(){ 
 
        $(window).scroll(function(){
            if ($(this).scrollTop() > 100) {
                $('.scrollup').fadeIn();
            } else {
                $('.scrollup').fadeOut();
            }
        }); 
 
        $('.scrollup').click(function(){
            $("html, body").animate({ scrollTop: 0 }, 600);
            return false;
        });
 
    });