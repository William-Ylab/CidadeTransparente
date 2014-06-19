// JavaScript Document
var scrollTop, window_h;

$(function(){
	scrollTop = $(window).scrollTop();
	window_h = $(window).height();
	styledImg();
})

$(window).scroll(function(){
	scrollTop = $(window).scrollTop();
	initProgressBarWithImage();
	progress_bars(true);
	trends();
    easePieChart();
})

$(window).resize(function(){
	window_h = $(window).height();
	progress_bars(true);
	initProgressBarWithImage();
})

$(window).bind('orientationchange', function() {
  myOrientResizeFunction()
});

function myOrientResizeFunction(){
	progress_bars();
	initProgressBarWithImage();
}

function initProgressBarWithImage(){
	if($('.progress_bars_with_image').length){
		$('.progress_bars_with_image').not('.visible').each(function(){
			var cur = $(this);
			if(scrollTop + window_h > cur.offset().top){
			
				cur.html('');
			
				var number = parseInt(cur.attr('data-number'));
				var value = parseInt(cur.attr('data-value'));
				var color = cur.attr('data-color');
				var icon = cur.attr('data-icon');
				var height = parseInt(cur.attr('data-height'));
				var letter_spacing = cur.attr('data-letterSpacing');
				if(!letter_spacing) letter_spacing = 0;
				
				cur.css({'font-size': height + 'px', 'line-height': height + 'px', 'letter-spacing': letter_spacing + 'em'});
				for(var i = 0; i < number; i++){
					var _item = $('<div class="item ' + icon + '"></div>');
					cur.append(_item);
				}
				cur.find('.item').each(function(index, element){
					if(index < value) {
						var cur_item = $(this);
						cur_item.addClass('active')
						setTimeout(function(){
							cur_item.css('color', color);
						}, index*300)
					}
				})
				
				setTimeout(function(){
					var item_width = _item.width();
					var global_width = item_width * number;
					if(global_width > cur.width()) cur.find('.item.active:last').addClass('break');
				}, 200);
				
				cur.addClass('visible');
				
			} else {
				var _item = cur.find('.item');
				var number = parseInt(cur.attr('data-number'));
				cur.find('.item.break').removeClass('break');
				var item_width = _item.width();
				var global_width = item_width * number;
				if(global_width > cur.width()) cur.find('.item.active:last').addClass('break');
			}
		})
	}
}

function progress_bars(update){
	if($('.progress_bars').length){
		$('.progress_bars').not('.visible').each(function(){
			var cur = $(this);
			
			cur.css('width', 'auto');
			var width = cur.attr('data-width') ? parseInt(cur.attr('data-width')) : cur.parent().width();
			if(cur.hasClass('horizontal') && width < cur.parent().width()) width = cur.parent().width();
			var height = parseInt(cur.attr('data-height'));
			var color = cur.attr('data-color');
            var bg = cur.attr('data-bg');
			var title = cur.attr('data-title');
			var value = parseInt(cur.attr('data-value'))/100;
			if(!value) value = 1;
			
			color = hexDec(color);
			
			cur.css({'width': width, 'height': height})
			if(!bg) bg = '#ccc';
            cur.css({'background-color': bg});
			
			if(!update){
				cur.html('');
				var progress_line = $('<div class="progress_line"></div>').css('background-color', 'rgb(' + color + ')');
			
				if(title && title!='' && title!='false'){
					var title_block = $('<div class="title_block"></div>');
					 if(title != 'bottom') {
						 title_block.css({
							'height': height*2 - 2,
							'width': height*2 - 2,
							'margin-right': 1 - height
						});	
					 }
					if(cur.hasClass('style1')) {
						if(title == 'inner'){
							title_block.css({
								'border-color': 'rgb(' + color + ')',
								'color': 'rgb(' + color + ')',
								'line-height': height*2 - 6 + 'px',
								'font-size': cur.hasClass('vertical') ? width/1.7 : height/1.7
							});
						} else if(title == 'bottom'){
							title_block.css({
								'color': 'rgb(' + color + ')',
								'font-size': cur.hasClass('vertical') ? width*0.35 : height*0.35,
								'line-height': cur.hasClass('vertical') ? (width*0.35 + 20 + 'px') : (height*0.35 + 20 + 'px')
							});
							cur.css('margin-bottom', cur.hasClass('vertical') ? width*0.6 : height*0.6)	
						}
					}
					if(cur.hasClass('style2')) {
						if(title == 'inner'){
							title_block.css({
								'background': 'rgb(' + color + ')',
								'margin-bottom': 1 - height,
								'line-height': height*2 - 2 + 'px',
								'font-size': cur.hasClass('vertical') ? width/1.7 : height/1.7
							});
						} else if(title == 'bottom'){
							title_block.css({
								'color': 'rgb(' + color + ')',
								'font-size': cur.hasClass('vertical') ? width*0.35 : height*0.35,
								'line-height': cur.hasClass('vertical') ? (width*0.35 + 20 + 'px') : (height*0.35 + 20 + 'px')
							});
							cur.css('margin-bottom', cur.hasClass('vertical') ? width*0.6 : height*0.6)	
						}
					}
					cur.append(title_block);
				}
				
				cur.append(progress_line);
			} else {
				if(title && title!='' && title!='false'){
					var title_block = cur.find('.title_block');
				}
				var progress_line = cur.find('.progress_line');
			}
			
			if(cur.hasClass('vertical')){
				var progress_size = height*(1 - value);
				if(scrollTop + window_h > cur.offset().top){
					progress_line.animate(
						{'top': progress_size}, 
						{
							duration: 2000, 
							step: function(now, fx) {
								if(title && title!='' && title!='false'){
									var data = Math.round((1 - now/height) * 100);
									title_block.html(data + '%');
								}
							}	
						}
					);
					if(title && title!='' && title!='false' && title!='bottom'){
						title_block.animate({'top': progress_size}, 2000);
					}
					cur.addClass('visible');
				}
			} else {
				var progress_size = width*(1 - value);
				if(scrollTop + window_h > cur.offset().top){
					progress_line.animate(
						{'right': progress_size}, 
						{
							duration: 2000, 
							step: function(now, fx) {
								if(title && title!='' && title!='false'){
									var data = Math.round((1 - now/width) * 100);
									title_block.html(data + '%');
								}
							}	
						}
					);
					if(title && title!='' && title!='false' && title!='bottom'){
						title_block.animate({'right': progress_size}, 2000);
					}
					
					cur.addClass('visible');
				}
			}
		})
	}	
}

function easePieChart(){
	if($('.easy-pie-chart').length){
		$('.easy-pie-chart').not('.visible').each(function(){
			var cur = $(this);
			
			if(scrollTop + window_h > cur.offset().top){
                            
				cur.html('<span></span>');			
							
				var size = cur.attr('data-size');
				var trackColor = cur.attr('data-trackColor');
				var barColor = cur.attr('data-color');
				var bgColor = cur.attr('data-bgColor');
				var lineWidth = cur.attr('data-lineWidth');
				var lineCap = cur.attr('data-cap');
				var text = cur.attr('data-text');
                
                if(!bgColor) bgColor = '#cccccc';
				
                cur.easyPieChart({
                    text: text,
					animate: 2000,
					scaleColor: false,
					lineWidth: lineWidth,
					lineCap: lineCap,
					size: size,
					trackColor: bgColor,
					barColor: barColor
				});
				cur.addClass('visible');
				$(".pieexemple").fadeIn();
			}
		});
	}
}





function processBox(){
	if($(".styled_list").length){
		$('.styled_list').each(function(){
			var cur = $(this);
			var colors = cur.attr('data-color');
			var type = cur.attr('data-type');
			
			if(colors){
				colors = colors.split(',');
			} else {
				colors = ['#ffffff']
			}
			cur.children('li').each(function(index, element){
				if($(this).find('.text').length) {
					var html = $(this).find('.text').html();
				} else {
					var html = $(this).html();
				}
				$(this).html('');
				var text_block = $('<div class="text">' + html + '</div>');
				
				if(type == 'process_box'){
					var num = index + 1 + '';
				    //if(num.length < 2) num = '0' + num;
					if (num.length < 2) num = num;
					var num_background = colors[index%colors.length];
					var num_block = $('<div class="num" style="background-color: ' + num_background + '">' + num + '</div>');
				} else if(type == 'large_numbers'){
					var num = index + 1 + '.';
					var num_color = colors[index%colors.length];
					var num_block = $('<div class="num" style="color: ' + num_color + '">' + num + '</div>');
				}
				
				$(this).append(num_block);
				$(this).append(text_block);
			})
		})
	}	
}

function infoDeviders(){
	if($(".info_divider").length){
		$(".info_divider").each(function(){
			var cur = $(this);
			var width = cur.attr('data-width');
			var height = cur.attr('data-height');
			if(width) cur.css('width', width);
			if(height) cur.css('height', height);
			
			var label = $('<i style=""></i>');
			if(cur.hasClass('vertical')) {
				if(width) var label_size = width;
			} else {
				if(height) var label_size = height;
			}
			if(label_size) label.css({
				'width': label_size,
				'height': label_size,
				'line-height': label_size + 'px'
			})
			cur.append(label);
		})
	}	
}

function trends(){
	if($('.trend').length){
		$('.trend').each(function(){
			var cur = $(this);
			
			var padding_bottom = 100;
			
			var height = cur.outerHeight() - padding_bottom;
			var width = cur.width();
			
			var color = cur.attr('data-color');
			var value = parseInt(cur.attr('data-value'));
			var trend = cur.attr('data-trend');
			var name = cur.attr('data-name');
			
			var arrow_w = height*0.356;
			var border_w_bottom = height*0.48;
			
			var arrow = $("<div class='arrow'></div>");
			var arrow_i = $("<i />");
			var value_block = $("<div class='value'></div>");
			var name_block = $("<div class='name'>" + name + "</div>");
			
			if(scrollTop + window_h > cur.offset().top && !cur.hasClass('visible')){
				value_block.css({
					'color': color
				})
				name_block.css({
					'color': color
				})
				
				arrow.append(arrow_i);
				cur.append(arrow);
				cur.append(value_block);
				cur.append(name_block);
				
				arrow.css({
					'width': arrow_w,
					'margin-left' : '-' + arrow_w/2 + 'px',
					'background': color,
					'height': height*0.52
				})
				arrow_i.css({
					'border-width': arrow_w + 'px',
					'margin-left': -arrow_w + 'px'
				})
				
				if(trend == 'up'){
					arrow.css({
						'bottom': -height
					})
					arrow_i.css({
						'border-bottom-color': color,
						'border-bottom-width' : border_w_bottom + 'px',
						'bottom': '100%'
					})
					arrow.animate({'bottom': padding_bottom}, {
						duration: 2000,
						step: function(now, fx) {
							var data = Math.round((now + height)/(height + padding_bottom) * value);
							value_block.html(data + '%');
						}					
					})
				} else if(trend == 'down'){
					arrow.css({
						'top': -height	
					})
					arrow_i.css({
						'border-top-color': color,
						'border-top-width' : border_w_bottom + 'px',
						'top': '100%'
					})
					arrow.animate({'top': 0}, {
						duration: 2000,
						step: function(now, fx) {
							var data = Math.round((1 - now/(-height)) * value);
							value_block.html(data + '%');
						}						
					})
				}
				cur.addClass('visible');
			}
			
		})
	}	
}


function hexDec(color){
	var m=color.slice(1).match(/.{2}/g);
	 
	m[0]=parseInt(m[0], 16);
	m[1]=parseInt(m[1], 16);
	m[2]=parseInt(m[2], 16);
	 
	return m[0] + ', ' + m[1] + ', ' + m[2];
	
};

function styledImg(){
	$('img.styled').each(function(){
		$(this).wrap($('<div class="styled_img_container"/>'))
	})	
}