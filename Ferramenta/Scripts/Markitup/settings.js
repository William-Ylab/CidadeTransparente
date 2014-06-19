var mySettings = {
    nameSpace: "textile",
    onShiftEnter: { keepDefault: false, replaceWith: '<br />\n' },
    onCtrlEnter: { keepDefault: false, openWith: '\n<p>', closeWith: '</p>' },
    onTab: { keepDefault: false, replaceWith: '    ' },
    markupSet: [
		{ key: 'B', openWith: '(!(<strong>|!|<b>)!)', closeWith: '(!(</strong>|!|</b>)!)', className: "markitUpButtonBold" },
		{ key: 'I', openWith: '(!(<em>|!|<i>)!)', closeWith: '(!(</em>|!|</i>)!)', className: "markitUpButtonItalic" },
		{ key: 'U', openWith: '<u>', closeWith: '</u>', className: "markitUpButtonUnderline" }
        //{ name: 'Stroke through', key: 'S', openWith: '<del>', closeWith: '</del>' },
		//{ separator: '---------------' },
		//{ name: 'Bulleted List', openWith: '    <li>', closeWith: '</li>', multiline: true, openBlockWith: '<ul>\n', closeBlockWith: '\n</ul>' },
		//{ name: 'Numeric List', openWith: '    <li>', closeWith: '</li>', multiline: true, openBlockWith: '<ol>\n', closeBlockWith: '\n</ol>' },
		//{ separator: '---------------' },
		//{ name: 'Picture', key: 'P', replaceWith: '<img src="[![Source:!:http://]!]" alt="[![Alternative text]!]" />' },
		//{ name: 'Link', key: 'L', openWith: '<a href="[![Link:!:http://]!]"(!( title="[![Title]!]")!)>', closeWith: '</a>', placeHolder: 'Your text to link...' },
		//{ separator: '---------------' },
		//{ name: 'Clean', className: 'clean', replaceWith: function (markitup) { return markitup.selection.replace(/<(.*?)>/g, "") } },
		//{ name: 'Preview', className: 'preview', call: 'preview' }
    ]
}
