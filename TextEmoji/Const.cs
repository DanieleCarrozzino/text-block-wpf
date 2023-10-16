﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TextEmoji
{
    internal static class Const
    {
        public static double FontSize = 20.0;

        private static String IANA_TOP_LEVEL_DOMAINS =
        "(?:"
        + "(?:aaa|aarp|abb|abbott|abbvie|abc|able|abogado|abudhabi|academy|accenture|accountant"
        + "|accountants|aco|actor|ads|adult|aeg|aero|aetna|afl|africa|agakhan|agency|aig|airbus"
        + "|airforce|airtel|akdn|alibaba|alipay|allfinanz|allstate|ally|alsace|alstom|amazon|americanexpress"
        + "|americanfamily|amex|amfam|amica|amsterdam|analytics|android|anquan|anz|aol|apartments"
        + "|app|apple|aquarelle|arab|aramco|archi|army|arpa|art|arte|asda|asia|associates|athleta"
        + "|attorney|auction|audi|audible|audio|auspost|author|auto|autos|avianca|aws|axa|azure"
        + "|a[cdefgilmoqrstuwxz])"
        + "|(?:baby|baidu|banamex|bananarepublic|band|bank|bar|barcelona|barclaycard|barclays"
        + "|barefoot|bargains|baseball|basketball|bauhaus|bayern|bbc|bbt|bbva|bcg|bcn|beats|beauty"
        + "|beer|bentley|berlin|best|bestbuy|bet|bharti|bible|bid|bike|bing|bingo|bio|biz|black"
        + "|blackfriday|blockbuster|blog|bloomberg|blue|bms|bmw|bnpparibas|boats|boehringer|bofa"
        + "|bom|bond|boo|book|booking|bosch|bostik|boston|bot|boutique|box|bradesco|bridgestone"
        + "|broadway|broker|brother|brussels|build|builders|business|buy|buzz|bzh|b[abdefghijmnorstvwyz])"
        + "|(?:cab|cafe|cal|call|calvinklein|cam|camera|camp|canon|capetown|capital|capitalone"
        + "|car|caravan|cards|care|career|careers|cars|casa|case|cash|casino|cat|catering|catholic"
        + "|cba|cbn|cbre|cbs|center|ceo|cern|cfa|cfd|chanel|channel|charity|chase|chat|cheap|chintai"
        + "|christmas|chrome|church|cipriani|circle|cisco|citadel|citi|citic|city|cityeats|claims"
        + "|cleaning|click|clinic|clinique|clothing|cloud|club|clubmed|coach|codes|coffee|college"
        + "|cologne|com|comcast|commbank|community|company|compare|computer|comsec|condos|construction"
        + "|consulting|contact|contractors|cooking|cool|coop|corsica|country|coupon|coupons|courses"
        + "|cpa|credit|creditcard|creditunion|cricket|crown|crs|cruise|cruises|cuisinella|cymru"
        + "|cyou|c[acdfghiklmnoruvwxyz])"
        + "|(?:dabur|dad|dance|data|date|dating|datsun|day|dclk|dds|deal|dealer|deals|degree"
        + "|delivery|dell|deloitte|delta|democrat|dental|dentist|desi|design|dev|dhl|diamonds|diet"
        + "|digital|direct|directory|discount|discover|dish|diy|dnp|docs|doctor|dog|domains|dot"
        + "|download|drive|dtv|dubai|dunlop|dupont|durban|dvag|dvr|d[ejkmoz])"
        + "|(?:earth|eat|eco|edeka|edu|education|email|emerck|energy|engineer|engineering|enterprises"
        + "|epson|equipment|ericsson|erni|esq|estate|etisalat|eurovision|eus|events|exchange|expert"
        + "|exposed|express|extraspace|e[cegrstu])"
        + "|(?:fage|fail|fairwinds|faith|family|fan|fans|farm|farmers|fashion|fast|fedex|feedback"
        + "|ferrari|ferrero|fidelity|fido|film|final|finance|financial|fire|firestone|firmdale"
        + "|fish|fishing|fit|fitness|flickr|flights|flir|florist|flowers|fly|foo|food|football"
        + "|ford|forex|forsale|forum|foundation|fox|free|fresenius|frl|frogans|frontdoor|frontier"
        + "|ftr|fujitsu|fun|fund|furniture|futbol|fyi|f[ijkmor])"
        + "|(?:gal|gallery|gallo|gallup|game|games|gap|garden|gay|gbiz|gdn|gea|gent|genting"
        + "|george|ggee|gift|gifts|gives|giving|glass|gle|global|globo|gmail|gmbh|gmo|gmx|godaddy"
        + "|gold|goldpoint|golf|goo|goodyear|goog|google|gop|got|gov|grainger|graphics|gratis|green"
        + "|gripe|grocery|group|guardian|gucci|guge|guide|guitars|guru|g[abdefghilmnpqrstuwy])"
        + "|(?:hair|hamburg|hangout|haus|hbo|hdfc|hdfcbank|health|healthcare|help|helsinki|here"
        + "|hermes|hiphop|hisamitsu|hitachi|hiv|hkt|hockey|holdings|holiday|homedepot|homegoods"
        + "|homes|homesense|honda|horse|hospital|host|hosting|hot|hotels|hotmail|house|how|hsbc"
        + "|hughes|hyatt|hyundai|h[kmnrtu])"
        + "|(?:ibm|icbc|ice|icu|ieee|ifm|ikano|imamat|imdb|immo|immobilien|inc|industries|infiniti"
        + "|info|ing|ink|institute|insurance|insure|int|international|intuit|investments|ipiranga"
        + "|irish|ismaili|ist|istanbul|itau|itv|i[delmnoqrst])"
        + "|(?:jaguar|java|jcb|jeep|jetzt|jewelry|jio|jll|jmp|jnj|jobs|joburg|jot|joy|jpmorgan"
        + "|jprs|juegos|juniper|j[emop])"
        + "|(?:kaufen|kddi|kerryhotels|kerrylogistics|kerryproperties|kfh|kia|kids|kim|kinder"
        + "|kindle|kitchen|kiwi|koeln|komatsu|kosher|kpmg|kpn|krd|kred|kuokgroup|kyoto|k[eghimnprwyz])"
        + "|(?:lacaixa|lamborghini|lamer|lancaster|land|landrover|lanxess|lasalle|lat|latino"
        + "|latrobe|law|lawyer|lds|lease|leclerc|lefrak|legal|lego|lexus|lgbt|lidl|life|lifeinsurance"
        + "|lifestyle|lighting|like|lilly|limited|limo|lincoln|link|lipsy|live|living|llc|llp|loan"
        + "|loans|locker|locus|lol|london|lotte|lotto|love|lpl|lplfinancial|ltd|ltda|lundbeck|luxe"
        + "|luxury|l[abcikrstuvy])"
        + "|(?:madrid|maif|maison|makeup|man|management|mango|map|market|marketing|markets|marriott"
        + "|marshalls|mattel|mba|mckinsey|med|media|meet|melbourne|meme|memorial|men|menu|merckmsd"
        + "|miami|microsoft|mil|mini|mint|mit|mitsubishi|mlb|mls|mma|mobi|mobile|moda|moe|moi|mom"
        + "|monash|money|monster|mormon|mortgage|moscow|moto|motorcycles|mov|movie|msd|mtn|mtr"
        + "|museum|music|m[acdeghklmnopqrstuvwxyz])"
        + "|(?:nab|nagoya|name|natura|navy|nba|nec|net|netbank|netflix|network|neustar|new|news"
        + "|next|nextdirect|nexus|nfl|ngo|nhk|nico|nike|nikon|ninja|nissan|nissay|nokia|norton"
        + "|now|nowruz|nowtv|nra|nrw|ntt|nyc|n[acefgilopruz])"
        + "|(?:obi|observer|office|okinawa|olayan|olayangroup|oldnavy|ollo|omega|one|ong|onl"
        + "|online|ooo|open|oracle|orange|org|organic|origins|osaka|otsuka|ott|ovh|om)"
        + "|(?:page|panasonic|paris|pars|partners|parts|party|pay|pccw|pet|pfizer|pharmacy|phd"
        + "|philips|phone|photo|photography|photos|physio|pics|pictet|pictures|pid|pin|ping|pink"
        + "|pioneer|pizza|place|play|playstation|plumbing|plus|pnc|pohl|poker|politie|porn|post"
        + "|pramerica|praxi|press|prime|pro|prod|productions|prof|progressive|promo|properties"
        + "|property|protection|pru|prudential|pub|pwc|p[aefghklmnrstwy])"
        + "|(?:qpon|quebec|quest|qa)"
        + "|(?:racing|radio|read|realestate|realtor|realty|recipes|red|redstone|redumbrella"
        + "|rehab|reise|reisen|reit|reliance|ren|rent|rentals|repair|report|republican|rest|restaurant"
        + "|review|reviews|rexroth|rich|richardli|ricoh|ril|rio|rip|rocher|rocks|rodeo|rogers|room"
        + "|rsvp|rugby|ruhr|run|rwe|ryukyu|r[eosuw])"
        + "|(?:saarland|safe|safety|sakura|sale|salon|samsclub|samsung|sandvik|sandvikcoromant"
        + "|sanofi|sap|sarl|sas|save|saxo|sbi|sbs|sca|scb|schaeffler|schmidt|scholarships|school"
        + "|schule|schwarz|science|scot|search|seat|secure|security|seek|select|sener|services"
        + "|seven|sew|sex|sexy|sfr|shangrila|sharp|shaw|shell|shia|shiksha|shoes|shop|shopping"
        + "|shouji|show|showtime|silk|sina|singles|site|ski|skin|sky|skype|sling|smart|smile|sncf"
        + "|soccer|social|softbank|software|sohu|solar|solutions|song|sony|soy|spa|space|sport"
        + "|spot|srl|stada|staples|star|statebank|statefarm|stc|stcgroup|stockholm|storage|store"
        + "|stream|studio|study|style|sucks|supplies|supply|support|surf|surgery|suzuki|swatch"
        + "|swiss|sydney|systems|s[abcdeghijklmnorstuvxyz])"
        + "|(?:tab|taipei|talk|taobao|target|tatamotors|tatar|tattoo|tax|taxi|tci|tdk|team|tech"
        + "|technology|tel|temasek|tennis|teva|thd|theater|theatre|tiaa|tickets|tienda|tips|tires"
        + "|tirol|tjmaxx|tjx|tkmaxx|tmall|today|tokyo|tools|top|toray|toshiba|total|tours|town"
        + "|toyota|toys|trade|trading|training|travel|travelers|travelersinsurance|trust|trv|tube"
        + "|tui|tunes|tushu|tvs|t[cdfghjklmnortvwz])"
        + "|(?:ubank|ubs|unicom|university|uno|uol|ups|u[agksyz])"
        + "|(?:vacations|vana|vanguard|vegas|ventures|verisign|versicherung|vet|viajes|video"
        + "|vig|viking|villas|vin|vip|virgin|visa|vision|viva|vivo|vlaanderen|vodka|volkswagen"
        + "|volvo|vote|voting|voto|voyage|v[aceginu])"
        + "|(?:wales|walmart|walter|wang|wanggou|watch|watches|weather|weatherchannel|webcam"
        + "|weber|website|wed|wedding|weibo|weir|whoswho|wien|wiki|williamhill|win|windows|wine"
        + "|winners|wme|wolterskluwer|woodside|work|works|world|wow|wtc|wtf|w[fs])"
        + "|(?:xbox|xerox|xfinity|xihuan|xin|xn\\-\\-11b4c3d|xn\\-\\-1ck2e1b|xn\\-\\-1qqw23a|xn\\-\\-2scrj9c"
        + "|xn\\-\\-30rr7y|xn\\-\\-3bst00m|xn\\-\\-3ds443g|xn\\-\\-3e0b707e|xn\\-\\-3hcrj9c|xn\\-\\-3pxu8k"
        + "|xn\\-\\-42c2d9a|xn\\-\\-45br5cyl|xn\\-\\-45brj9c|xn\\-\\-45q11c|xn\\-\\-4dbrk0ce|xn\\-\\-4gbrim"
        + "|xn\\-\\-54b7fta0cc|xn\\-\\-55qw42g|xn\\-\\-55qx5d|xn\\-\\-5su34j936bgsg|xn\\-\\-5tzm5g"
        + "|xn\\-\\-6frz82g|xn\\-\\-6qq986b3xl|xn\\-\\-80adxhks|xn\\-\\-80ao21a|xn\\-\\-80aqecdr1a"
        + "|xn\\-\\-80asehdb|xn\\-\\-80aswg|xn\\-\\-8y0a063a|xn\\-\\-90a3ac|xn\\-\\-90ae|xn\\-\\-90ais"
        + "|xn\\-\\-9dbq2a|xn\\-\\-9et52u|xn\\-\\-9krt00a|xn\\-\\-b4w605ferd|xn\\-\\-bck1b9a5dre4c"
        + "|xn\\-\\-c1avg|xn\\-\\-c2br7g|xn\\-\\-cck2b3b|xn\\-\\-cckwcxetd|xn\\-\\-cg4bki|xn\\-\\-clchc0ea0b2g2a9gcd"
        + "|xn\\-\\-czr694b|xn\\-\\-czrs0t|xn\\-\\-czru2d|xn\\-\\-d1acj3b|xn\\-\\-d1alf|xn\\-\\-e1a4c"
        + "|xn\\-\\-eckvdtc9d|xn\\-\\-efvy88h|xn\\-\\-fct429k|xn\\-\\-fhbei|xn\\-\\-fiq228c5hs"
        + "|xn\\-\\-fiq64b|xn\\-\\-fiqs8s|xn\\-\\-fiqz9s|xn\\-\\-fjq720a|xn\\-\\-flw351e|xn\\-\\-fpcrj9c3d"
        + "|xn\\-\\-fzc2c9e2c|xn\\-\\-fzys8d69uvgm|xn\\-\\-g2xx48c|xn\\-\\-gckr3f0f|xn\\-\\-gecrj9c"
        + "|xn\\-\\-gk3at1e|xn\\-\\-h2breg3eve|xn\\-\\-h2brj9c|xn\\-\\-h2brj9c8c|xn\\-\\-hxt814e"
        + "|xn\\-\\-i1b6b1a6a2e|xn\\-\\-imr513n|xn\\-\\-io0a7i|xn\\-\\-j1aef|xn\\-\\-j1amh|xn\\-\\-j6w193g"
        + "|xn\\-\\-jlq480n2rg|xn\\-\\-jvr189m|xn\\-\\-kcrx77d1x4a|xn\\-\\-kprw13d|xn\\-\\-kpry57d"
        + "|xn\\-\\-kput3i|xn\\-\\-l1acc|xn\\-\\-lgbbat1ad8j|xn\\-\\-mgb9awbf|xn\\-\\-mgba3a3ejt"
        + "|xn\\-\\-mgba3a4f16a|xn\\-\\-mgba7c0bbn0a|xn\\-\\-mgbaakc7dvf|xn\\-\\-mgbaam7a8h|xn\\-\\-mgbab2bd"
        + "|xn\\-\\-mgbah1a3hjkrd|xn\\-\\-mgbai9azgqp6j|xn\\-\\-mgbayh7gpa|xn\\-\\-mgbbh1a|xn\\-\\-mgbbh1a71e"
        + "|xn\\-\\-mgbc0a9azcg|xn\\-\\-mgbca7dzdo|xn\\-\\-mgbcpq6gpa1a|xn\\-\\-mgberp4a5d4ar|xn\\-\\-mgbgu82a"
        + "|xn\\-\\-mgbi4ecexp|xn\\-\\-mgbpl2fh|xn\\-\\-mgbt3dhd|xn\\-\\-mgbtx2b|xn\\-\\-mgbx4cd0ab"
        + "|xn\\-\\-mix891f|xn\\-\\-mk1bu44c|xn\\-\\-mxtq1m|xn\\-\\-ngbc5azd|xn\\-\\-ngbe9e0a|xn\\-\\-ngbrx"
        + "|xn\\-\\-node|xn\\-\\-nqv7f|xn\\-\\-nqv7fs00ema|xn\\-\\-nyqy26a|xn\\-\\-o3cw4h|xn\\-\\-ogbpf8fl"
        + "|xn\\-\\-otu796d|xn\\-\\-p1acf|xn\\-\\-p1ai|xn\\-\\-pgbs0dh|xn\\-\\-pssy2u|xn\\-\\-q7ce6a"
        + "|xn\\-\\-q9jyb4c|xn\\-\\-qcka1pmc|xn\\-\\-qxa6a|xn\\-\\-qxam|xn\\-\\-rhqv96g|xn\\-\\-rovu88b"
        + "|xn\\-\\-rvc1e0am3e|xn\\-\\-s9brj9c|xn\\-\\-ses554g|xn\\-\\-t60b56a|xn\\-\\-tckwe|xn\\-\\-tiq49xqyj"
        + "|xn\\-\\-unup4y|xn\\-\\-vermgensberater\\-ctb|xn\\-\\-vermgensberatung\\-pwb|xn\\-\\-vhquv"
        + "|xn\\-\\-vuq861b|xn\\-\\-w4r85el8fhu5dnra|xn\\-\\-w4rs40l|xn\\-\\-wgbh1c|xn\\-\\-wgbl6a"
        + "|xn\\-\\-xhq521b|xn\\-\\-xkc2al3hye2a|xn\\-\\-xkc2dl3a5ee0h|xn\\-\\-y9a3aq|xn\\-\\-yfro4i67o"
        + "|xn\\-\\-ygbi2ammx|xn\\-\\-zfr164b|xxx|xyz)"
        + "|(?:yachts|yahoo|yamaxun|yandex|yodobashi|yoga|yokohama|you|youtube|yun|y[et])"
        + "|(?:zappos|zara|zero|zip|zone|zuerich|z[amw]))";


        private static String PUNYCODE_TLD = "xn\\-\\-[\\w\\-]{0,58}\\w";

        private static String STRICT_TLD = "(?:" +
            IANA_TOP_LEVEL_DOMAINS + "|" + PUNYCODE_TLD + ")";

        private static String LABEL_CHAR = "a-zA-Z0-9";

        private static String IRI_LABEL =
            "[" + LABEL_CHAR + "](?:[" + LABEL_CHAR + "_\\-]{0,61}[" + LABEL_CHAR + "]){0,1}";

        private static String HOST_NAME = "(" + IRI_LABEL + "\\.)+" + STRICT_TLD;

        private static String IP_ADDRESS_STRING =
            "((25[0-5]|2[0-4][0-9]|[0-1][0-9]{2}|[1-9][0-9]|[1-9])\\.(25[0-5]|2[0-4]"
            + "[0-9]|[0-1][0-9]{2}|[1-9][0-9]|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]"
            + "[0-9]{2}|[1-9][0-9]|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1][0-9]{2}"
            + "|[1-9][0-9]|[0-9]))";

        private static String DOMAIN_NAME_STR = "(" + HOST_NAME + "|" + IP_ADDRESS_STRING + ")";

        private static String USER_INFO = "(?:[a-zA-Z0-9\\$\\-\\.\\+\\!\\*\\'\\(\\)"
            + "\\,\\;\\?\\&\\=]|(?:\\%[a-fA-F0-9]{2})){1,64}(?:\\:(?:[a-zA-Z0-9\\$\\-"
            + "\\.\\+\\!\\*\\'\\(\\)\\,\\;\\?\\&\\=]|(?:\\%[a-fA-F0-9]{2})){1,25})?\\@";

        private static String PROTOCOL = "(?i:http|https|rtsp|ftp)://";

        private static String PORT_NUMBER = "\\:\\d{1,5}";

        private static String PATH_AND_QUERY = "[/\\?](?:(?:[" + LABEL_CHAR
            + ";/\\?:@&=#~"  // plus optional query params
            + "\\-\\.\\+!\\*'\\(\\),_\\$])|(?:%[a-fA-F0-9]{2}))*";

        private static String WORD_BOUNDARY = "(?:\\b|$|^)";

        public static String WEB_URL = "("
            + "("
            + "(?:" + PROTOCOL + "(?:" + USER_INFO + ")?" + ")?"
            + "(?:" + DOMAIN_NAME_STR + ")"
            + "(?:" + PORT_NUMBER + ")?"
            + ")"
            + "(" + PATH_AND_QUERY + ")?"
            + WORD_BOUNDARY
            + ")";

        public static String EMAIL_ADDRESS =
                "[a-zA-Z0-9\\+\\.\\_\\%\\-\\+]{1,256}" +
                "\\@" +
                "[a-zA-Z0-9][a-zA-Z0-9\\-]{0,64}" +
                "(" +
                    "\\." +
                    "[a-zA-Z0-9][a-zA-Z0-9\\-]{0,25}" +
                ")+";
    }
}
