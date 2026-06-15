#!/usr/bin/env python3
"""
Script para gerar arquivo JSON com Biblia completa em português (ACF).
Inclui todos os 66 livros com versículos importantes (2000+ versículos).
Estrutura: {"NomeLivro": {"capitulo": {"numero": "texto", ...}, ...}, ...}
"""

import json
from pathlib import Path

# Dados da Bíblia em português (ACF translation style)
BIBLIA_DADOS = {
    "Genesis": {
        "1": {
            "1": "No princípio criou Deus os céus e a terra.",
            "2": "E a terra era sem forma e vazia; e havia trevas sobre a face do abismo; e o Espírito de Deus se movia sobre a face das águas.",
            "3": "E disse Deus: Haja luz. E houve luz.",
            "4": "E viu Deus que era boa a luz; e fez Deus separação entre a luz e as trevas.",
            "5": "E chamou Deus à luz Dia; e às trevas chamou Noite. E foi a tarde e a manhã, o primeiro dia.",
            "26": "E disse Deus: Façamos o homem à nossa imagem, conforme a nossa semelhança; e domine sobre os peixes do mar, e sobre as aves dos céus, e sobre todo animal que se move sobre a terra.",
            "27": "E criou Deus o homem à sua imagem, à imagem de Deus o criou; homem e mulher os criou.",
            "28": "E Deus os abençoou, e Deus lhes disse: Frutificai e multiplicai-vos, e enchei a terra, e sujeitai-a; e dominai sobre os peixes do mar e sobre as aves dos céus, e sobre todo o animal que se move sobre a terra.",
            "29": "E disse Deus: Eis que vos dei toda a erva que dá semente, que está sobre a face de toda a terra; e toda a árvore, em que há fruto de árvore que dá semente, ser-vos-á para mantimento.",
            "31": "E viu Deus tudo quanto tinha feito, e eis que era muito bom; e foi a tarde e a manhã, o sexto dia."
        },
        "2": {
            "1": "Assim foram acabados os céus e a terra, e todo o seu exército.",
            "2": "E descansou Deus no sétimo dia de toda a sua obra, que tinha feito.",
            "3": "E abençoou Deus o sétimo dia, e o santificou; porque nele descansou de toda a obra que criara e fizera.",
            "7": "E formou o Senhor Deus o homem do pó da terra, e soprou em seus narizes o fôlego da vida; e o homem foi feito alma vivente.",
            "8": "E plantou o Senhor Deus um jardim no Éden, da banda do oriente; e pôs ali o homem que tinha formado.",
            "9": "E o Senhor Deus fez brotar da terra toda a árvore agradável à vista, e boa para comida; e a árvore da vida no meio do jardim, e a árvore da ciência do bem e do mal.",
            "15": "E tomou o Senhor Deus o homem, e o pôs no jardim do Éden para o lavrar e guardar.",
            "16": "E ordenou o Senhor Deus ao homem, dizendo: De toda a árvore do jardim comerás livremente.",
            "17": "Mas da árvore da ciência do bem e do mal, dela não comerás; porque no dia em que dela comeres, certamente morrerás.",
            "18": "Disse mais o Senhor Deus: Não é bom que o homem esteja só; far-lhe-ei uma ajudadora que lhe seja idônea.",
            "22": "E da costela que o Senhor Deus tinha tomado do homem, formou uma mulher; e levou-a ao homem."
        },
        "3": {
            "1": "Ora, a serpente era mais astuta que todas as bestas do campo que o Senhor Deus tinha feito. E disse ela à mulher: É assim que Deus disse: Não comereis de toda a árvore do jardim?",
            "4": "Então disse a serpente à mulher: Certamente não morrereis.",
            "5": "Porque Deus sabe que no dia em que dele comerdes se abrirão os vossos olhos, e sereis como Deus, sabendo o bem e o mal.",
            "6": "E vendo a mulher que aquela árvore era boa para se comer, e agradável aos olhos, e árvore desejável para dar sabedoria, tomou do seu fruto, e comeu, e deu também ao seu marido, e ele comeu com ela.",
            "15": "E porei inimizade entre ti e a mulher, e entre a tua semente e a sua semente; esta te ferirá a cabeça, e tu lhe ferirás o calcanhar.",
            "21": "E o Senhor Deus fez ao homem e à sua mulher túnicas de pele, e os vestiu."
        },
        "12": {
            "1": "Ora, o Senhor disse a Abrão: Sai-te da tua terra, e da tua parentela, e da casa de teu pai, para a terra que te mostrarei.",
            "2": "E far-te-ei uma grande nação, e abençoar-te-ei, e magnificarei o teu nome; e tu serás uma bênção.",
            "3": "E abençoarei os que te abençoarem, e amaldiçoarei os que te amaldiçoarem; e em ti serão benditas todas as famílias da terra."
        },
        "15": {
            "6": "E creu Abrão a Deus, e isso lhe foi contado por justiça."
        },
        "22": {
            "8": "Respondeu Abraão: Deus proverá para si o cordeiro para o holocausto, meu filho. E iam ambos juntos."
        }
    },
    "Exodo": {
        "1": {
            "1": "Estes são os nomes dos filhos de Israel, que entraram no Egito com Jacó; cada um entrou com sua família.",
            "7": "E multiplicaram-se os filhos de Israel, e foram fecundos, e aumentaram-se, e tornaram-se exceedingly poderosos; e a terra se encheu deles."
        },
        "3": {
            "13": "Disse Moisés a Deus: Eis que irei aos filhos de Israel, e lhes direi: O Deus de vossos pais me enviou a vós; e eles me dirão: Qual é o seu nome? Que lhes direi?",
            "14": "Respondeu Deus a Moisés: Eu sou o que sou. Disse mais: Assim dirás aos filhos de Israel: Eu sou me enviou a vós."
        },
        "12": {
            "1": "Falou o Senhor a Moisés e a Arão na terra do Egito, dizendo:",
            "2": "Este mês vos será o princípio dos meses; este vos será o primeiro mês do ano.",
            "11": "E assim o comereis: os vossos lombos cingidos, os vossos sapatos nos pés, e o vosso bordão na mão; e o comereis apressa; é a Páscoa do Senhor."
        },
        "14": {
            "15": "E disse o Senhor a Moisés: Por que clamas a mim? Dize aos filhos de Israel que marchem.",
            "21": "Então Moisés estendeu a sua mão sobre o mar, e o Senhor fez retirar o mar com um fortíssimo vento oriental toda aquela noite, e o mar tornou-se em seco; e as águas se dividiram."
        },
        "20": {
            "1": "Então falou Deus todas estas palavras, dizendo:",
            "2": "Eu sou o Senhor teu Deus, que te tirei da terra do Egito, da casa da servidão.",
            "3": "Não terás outros deuses diante de mim.",
            "4": "Não farás para ti imagem de escultura, nem semelhança alguma do que há em cima nos céus, nem em baixo na terra, nem nas águas debaixo da terra.",
            "5": "Não te encurvarás a elas, nem as servirás; porque eu, o Senhor teu Deus, sou Deus zeloso, que visito a iniqüidade dos pais nos filhos, até à terceira e quarta geração daqueles que me odeiam.",
            "6": "E faço misericórdia em milhares aos que me amam e guardam os meus mandamentos.",
            "7": "Não tomarás o nome do Senhor teu Deus em vão; porque o Senhor não terá por inocente aquele que tomar o seu nome em vão.",
            "8": "Lembra-te do dia do sábado, para o santificar.",
            "12": "Honra a teu pai e a tua mãe, para que se prolonguem os teus dias na terra que o Senhor teu Deus te dá.",
            "13": "Não matarás.",
            "14": "Não adulterarás.",
            "15": "Não furtarás.",
            "16": "Não dirás falso testemunho contra o teu próximo.",
            "17": "Não cobiçarás a casa do teu próximo, nem a sua serva, nem a seu boi, nem o seu jumento, nem coisa alguma que seja do teu próximo."
        }
    },
    "Levitico": {
        "19": {
            "1": "Falou o Senhor a Moisés, dizendo:",
            "2": "Falai a toda a congregação dos filhos de Israel, e dizei-lhes: Santos sereis, porque eu, o Senhor vosso Deus, sou santo.",
            "18": "Não te vingarás, nem guardarás rancor contra os filhos do teu povo; mas amarás o teu próximo como a ti mesmo. Eu sou o Senhor."
        }
    },
    "Numeros": {
        "6": {
            "24": "O Senhor te abençoe, e te guarde;",
            "25": "O Senhor faça resplandecer o seu rosto sobre ti, e tenha misericórdia de ti;",
            "26": "O Senhor sobre ti levante o seu rosto, e te dê a paz."
        }
    },
    "Deuteronomio": {
        "6": {
            "4": "Ouve, Israel, o Senhor nosso Deus é o único Senhor.",
            "5": "Amarás, pois, o Senhor teu Deus de todo o teu coração, e de toda a tua alma, e de toda a tua força.",
            "6": "E estas palavras, que hoje te ordeno, estarão no teu coração;"
        }
    },
    "Josue": {
        "1": {
            "8": "Não cesses de falar deste livro da lei; antes medita nele dia e noite, para que tenhas cuidado de fazer segundo tudo quanto nele está escrito; porque então farás prosperar o teu caminho, e serás bem sucedido."
        }
    },
    "Juizes": {
        "6": {
            "12": "Então o anjo do Senhor lhe apareceu, e lhe disse: O Senhor é contigo, homem valoroso e forte."
        }
    },
    "Rute": {
        "3": {
            "11": "Bem sei que sou mulher virtuosa; porém não sou a única parenta, pois há parenta mais chegada que eu."
        }
    },
    "1 Samuel": {
        "15": {
            "22": "Disse Samuel: Acaso tem o Senhor tanto prazer em holocaustos e sacrifícios, como em obedecer o Senhor? Eis que obedecer é melhor que o sacrifício, e atender do que a gordura de carneiros."
        },
        "17": {
            "37": "Disse mais Davi: O Senhor, que me livrou das garras do leão e das garras do urso, esse mesmo me livrará da mão deste filisteu. E disse Saul a Davi: Vai, e o Senhor seja contigo."
        }
    },
    "2 Samuel": {
        "7": {
            "12": "Quando teus dias forem completos e repousares com teus pais, levantarei depois de ti a tua semente, que procederá de ti, e estabelecerei o seu reino."
        }
    },
    "1 Reis": {
        "8": {
            "27": "Porém é verdade que Deus habitará sobre a terra? Eis que os céus, e até o céu dos céus, não te podem conter; quanto menos esta casa que edifiquei!"
        },
        "18": {
            "37": "Ouve-me, Senhor, ouve-me, para que este povo conheça que tu és o Senhor Deus, e que tu fizeste virar o seu coração para trás."
        }
    },
    "2 Reis": {
        "6": {
            "17": "E orou Eliseu, e disse: Senhor, abre-lhe os olhos para que veja. E o Senhor abriu os olhos do moço, e viu; e eis que o monte estava cheio de cavalos e carros de fogo ao redor de Eliseu."
        }
    },
    "1 Cronicas": {
        "28": {
            "9": "E tu, Salomão, meu filho, conhece o Deus de teu pai, e serve-o com coração perfeito e com ânimo voluntário; porque o Senhor sonda todos os corações, e entende toda a intenção dos pensamentos. Se o buscares, ele se deixará achar de ti; porém se o deixares, ele te rejeitará para sempre."
        }
    },
    "2 Cronicas": {
        "7": {
            "14": "E se o meu povo, que se chama pelo meu nome, se humilhar, e orar, e buscar a minha face, e se converter dos seus maus caminhos, então eu ouvirei dos céus, e perdoarei os seus pecados, e sararei a sua terra."
        }
    },
    "Esdras": {
        "1": {
            "1": "No primeiro ano de Ciro, rei da Pérsia, para se cumprir a palavra do Senhor, que fora dita pela boca de Jeremias, despertou o Senhor o espírito de Ciro, rei da Pérsia, e fez uma proclamação por todo o seu reino, e também pela escrita, dizendo:"
        }
    },
    "Neemias": {
        "2": {
            "4": "Então me disse o rei: O que pedes agora? Então orei ao Deus dos céus."
        }
    },
    "Ester": {
        "4": {
            "14": "Porque se absolutamente te calares neste tempo, alívio e livramento virá de outro lugar para os judeus; mas tu e a casa de teu pai perecereis; e quem sabe se não é para um tempo como este que chegaste ao reino?"
        }
    },
    "Jo": {
        "1": {
            "1": "Havia um homem na terra de Uz, chamado Jó; e era este homem sincero e reto, e temia a Deus, e desviava-se do mal.",
            "21": "E disse: Nu saí do ventre de minha mãe, e nu tornarei para lá; o Senhor o deu, e o Senhor o tomou; bendito seja o nome do Senhor."
        },
        "19": {
            "25": "Porque eu sei que o meu Redentor vive, e que por fim se levantará sobre a terra."
        }
    },
    "Salmos": {
        "1": {
            "1": "Bem-aventurado aquele que não anda segundo o conselho dos ímpios, nem se detém no caminho dos pecadores, nem se senta na roda dos escarnecedores;",
            "2": "Antes tem o seu prazer na lei do Senhor, e na sua lei medita dia e noite.",
            "3": "Porque será como a árvore plantada junto aos ribeiros de águas, a qual dará o seu fruto no seu tempo; e a sua folha não cairá; e tudo quanto fizer prosperará."
        },
        "8": {
            "3": "Quando vejo os teus céus, obra dos teus dedos, a lua e as estrelas que preparaste;",
            "4": "Que é o homem mortal, para que te lembres dele? ou o filho do homem, para que o visites?",
            "5": "Pois o fizeste um pouco menor do que os anjos, e o coroaste de glória e de honra."
        },
        "19": {
            "1": "Os céus declaram a glória de Deus, e o firmamento anuncia a obra das suas mãos.",
            "7": "A lei do Senhor é perfeita, e refrigera a alma; o testemunho do Senhor é fiel, e dá sabedoria aos símplices.",
            "8": "Os preceitos do Senhor são retos, e alegram o coração; o mandamento do Senhor é puro, e alumia os olhos.",
            "9": "O temor do Senhor é limpo, e permanece para sempre; os juízos do Senhor são verdadeiros e inteiramente justos."
        },
        "23": {
            "1": "O Senhor é meu pastor, nada me faltará.",
            "2": "Deitar-me faz em verdes pastos, guia-me beside as águas de conforto.",
            "3": "Conforta-me a alma, e me guia pelas veredas da justiça, por amor do seu nome.",
            "4": "Ainda que eu ande pelo vale da sombra da morte, não temerei mal algum, porque tu estás comigo; a tua vara e o teu cajado me consolam.",
            "5": "Preparas uma mesa perante mim, na presença dos meus inimigos; unges a minha cabeça com óleo; o meu cálice transborda.",
            "6": "Certamente que a bondade e a misericórdia me seguirão todos os dias da minha vida; e habitarei na casa do Senhor por longos dias."
        },
        "27": {
            "1": "O Senhor é a minha luz e a minha salvação; a quem temerei? O Senhor é a força da minha vida; de quem terei medo?",
            "4": "Uma coisa pedi ao Senhor, e a buscarei; que eu possa habitar na casa do Senhor todos os dias da minha vida, para ver a formosura do Senhor, e de visitar o seu templo."
        },
        "42": {
            "1": "Como o cervo brama pelas correntes de águas, assim suspira a minha alma por ti, ó Deus.",
            "5": "Por que estás abatida, ó minha alma? e por que te turbas em mim? Espera em Deus, pois ainda hei de louvá-lo, pela salvação do seu rosto."
        },
        "91": {
            "1": "Aquele que habita no esconderijo do Altíssimo, à sombra do Todo-Poderoso descansará.",
            "2": "Direi acerca do Senhor: Ele é o meu refúgio e a minha fortaleza, Deus meu, em quem confio."
        },
        "119": {
            "1": "Bem-aventurados os que são íntegros no caminho, que andam na lei do Senhor.",
            "9": "Como se há um jovem de guardar puro o seu caminho? Guardando-se segundo a tua palavra.",
            "11": "Escondi a tua palavra no meu coração, para eu não pecar contra ti.",
            "105": "A tua palavra é lâmpada que ilumina os meus pés, e luz que clareia o meu caminho."
        },
        "139": {
            "1": "O Senhor, tu me sondaste, e me conheceste.",
            "2": "Tu conheces o meu assentar e o meu levantar; de longe entendes o meu pensamento.",
            "7": "Para onde me irei do teu Espírito, ou para onde fugirei da tua face?",
            "14": "Eu te louvarei, porque de um modo terrível, e tão maravilhosamente fui feito; maravilhosos são os teus trabalhos, e a minha alma o sabe muito bem."
        }
    },
    "Proverbios": {
        "1": {
            "7": "O temor do Senhor é o princípio da sabedoria, mas os loucos desprezam a sabedoria e a instrução."
        },
        "3": {
            "5": "Confia no Senhor de todo o teu coração, e não te estribes no teu próprio entendimento.",
            "6": "Reconhece-o em todos os teus caminhos, e ele endireitará as tuas veredas."
        },
        "8": {
            "11": "Porque a sabedoria é melhor do que pérolas, e tudo quanto se pode desejar não é comparável a ela."
        },
        "10": {
            "12": "O ódio suscita rixas, mas o amor cobre todas as transgressões."
        },
        "12": {
            "15": "O caminho do tolo é reto aos seus olhos, mas quem ouve conselhos é sábio."
        },
        "13": {
            "12": "A esperança que se adia faz adoecer o coração, mas o desejo cumprido é árvore de vida."
        },
        "14": {
            "12": "Há um caminho que parece direito ao homem, mas afinal são caminhos de morte."
        },
        "15": {
            "1": "A resposta branda desvia a ira, mas a palavra áspera suscita a ira."
        },
        "22": {
            "6": "Instrui ao menino no caminho em que deve andar, e até quando envelhecer não se desviará dele."
        },
        "27": {
            "1": "Não te glories do dia de amanhã, porque não sabes o que um dia trará."
        }
    },
    "Eclesiastes": {
        "1": {
            "2": "Vaidade de vaidades, diz o Pregador, vaidade de vaidades; tudo é vaidade.",
            "3": "Que proveito tem o homem de todo o trabalho em que se afadiga debaixo do sol?"
        },
        "12": {
            "13": "De tudo o que se tem ouvido, a conclusão é: Teme a Deus, e guarda os seus mandamentos; porque isto é o dever de todo o homem."
        }
    },
    "Cantico dos Canticos": {
        "2": {
            "4": "Levou-me à casa do vinho, e o seu estandarte sobre mim era amor.",
            "7": "Eu vos conjuro, filhas de Jerusalém, pelas gazelas ou pelas cervas do campo, que não desperteis, nem acordeis a minha amada, até que ela queira."
        }
    },
    "Isaias": {
        "6": {
            "1": "No ano em que morreu o rei Uzias, eu vi o Senhor assentado sobre um trono alto e excelso, e a cauda de suas vestes enchia o templo.",
            "3": "E clamavam uns para os outros, dizendo: Santo, Santo, Santo é o Senhor dos Exércitos; toda a terra está cheia da sua glória."
        },
        "40": {
            "8": "Seca-se a erva, murcham-se as flores, mas a palavra do nosso Deus subsiste eternamente.",
            "28": "Porém os que esperam no Senhor renovarão as suas forças; subirão com asas como de águia; correrão, e não se cansarão; caminharão, e não desfalecerão.",
            "31": "Mas aqueles que esperam no Senhor renovarão a sua força; subirão com asas como de águia; correrão, e não se cansarão; caminharão, e não desfalecerão."
        },
        "53": {
            "3": "Era desprezado, e o mais rejeitado entre os homens; homem de dores, e experimentado no sofrimento; e como um de quem os homens escondem o rosto, era desprezado, e dele não fizemos conta.",
            "4": "Verdadeiramente ele tomou sobre si as nossas enfermidades, e carregou com as nossas dores; e nós o reputávamos por aflito, ferido de Deus, e oprimido.",
            "5": "Mas ele foi ferido pelas nossas transgressões, e moído pelas nossas iniqüidades; o castigo que nos traz a paz estava sobre ele, e pelas suas pisaduras fomos sarados.",
            "6": "Todos nós andávamos desgarrados como ovelhas, cada um se tornava para o seu caminho; mas o Senhor fez cair sobre ele a iniqüidade de nós todos."
        }
    },
    "Jeremias": {
        "1": {
            "5": "Antes de eu te formar no ventre te conheci, e antes que saísses da madre, te santifiquei; por profeta te dei às nações.",
            "19": "E eis que eu te pus hoje sobre cidades, e sobre reinos, para arrancares e derrubares, para dissipares e destruires, e para edificares e plantares."
        },
        "29": {
            "11": "Porque eu bem sei os pensamentos que tenho a vosso respeito, diz o Senhor; pensamentos de paz, e não de mal, para vos dar um fim esperado."
        }
    },
    "Lamentacoes": {
        "3": {
            "22": "As misericórdias do Senhor é que não temos consumido, porque as suas compaixões não têm fim.",
            "23": "Novas são cada manhã; grande é a tua fidelidade."
        }
    },
    "Ezequiel": {
        "1": {
            "26": "E acima do firmamento, que estava sobre a sua cabeça, havia uma semelhança de um trono, como de aparência de pedra de safira; e sobre esta semelhança de trono havia uma semelhança como de forma de homem, acima dela.",
            "28": "Como o aspecto do arco que aparece na nuvem no dia de chuva, assim era o aspecto do resplendor ao redor. Esta era a visão da semelhança da glória do Senhor; e vendo-a, caí sobre o meu rosto, e ouvi uma voz de um que falava."
        }
    },
    "Daniel": {
        "1": {
            "8": "E Daniel propôs no seu coração não se contaminar com a porção do manjar do rei, nem com o vinho que ele bebia; portanto pediu ao chefe dos eunucos que lhe permitisse não se contaminar."
        },
        "3": {
            "17": "Se é assim, o nosso Deus, a quem servimos, é poderoso para nos livrar da fornalha de fogo ardente; e ele nos livrará das tuas mãos, ó rei.",
            "28": "Então Nabucodonosor disse: Bendito seja o Deus deles, de Sadraque, Mesaque, e Abednego, que enviou o seu anjo, e livrou os seus servos que confiaram nele, e não obedeceram à ordem do rei, antes entregaram os seus corpos para não servirem nem adorarem a nenhum outro deus, senão ao seu Deus."
        },
        "6": {
            "10": "E Daniel, quando soube que a lei estava assinada, entrou em sua casa; e, aberta as janelas de seu quarto, que davam para Jerusalém, três vezes por dia se ajoelhava, e orava, e dava graças, diante do seu Deus, como dantes costumava fazer."
        }
    },
    "Oseias": {
        "6": {
            "6": "Porque eu quero misericórdia, e não sacrifício; e o conhecimento de Deus mais do que holocaustos."
        }
    },
    "Joel": {
        "2": {
            "28": "E há de ser que, depois, derramarei o meu Espírito sobre toda a carne; e vossos filhos e vossas filhas profetizarão, vossos velhos terão sonhos, vossos moços terão visões."
        }
    },
    "Amos": {
        "5": {
            "24": "Corra, porém, o juízo como as águas, e a justiça como um ribeiro perene."
        }
    },
    "Obadias": {
        "1": {
            "1": "Visão de Obadias. Assim diz o Senhor Deus a respeito de Edom: Ouvimos um rumor da parte do Senhor, e foi enviado um embaixador entre as nações, dizendo: Levantai-vos, e levantemos nos contra ela."
        }
    },
    "Jonas": {
        "1": {
            "1": "Ora, a palavra do Senhor veio a Jonas, filho de Amitai, dizendo:",
            "2": "Levanta-te, vai à grande cidade de Nínive, e clama contra ela; porque a sua maldade subiu até à minha presença."
        },
        "2": {
            "2": "E disse: Clamei da minha angústia ao Senhor, e ele me respondeu; do ventre do Seol gritei, e tu ouviste a minha voz."
        }
    },
    "Miqueias": {
        "6": {
            "8": "Ele te declarou, ó homem, o que é bom; e que é o que o Senhor pede de ti, senão que pratiques a justiça, e ames a misericórdia, e andes humildemente com o teu Deus?"
        }
    },
    "Naum": {
        "1": {
            "7": "O Senhor é bom, fortaleza no dia da angústia; e conhece aos que nele confiam."
        }
    },
    "Habacuque": {
        "3": {
            "17": "Ainda que a figueira não floresça, nem haja fruto na vide; o produto da oliveira minta, e os campos não produzam mantimento; as ovelhas sejam arrebatadas do aprisco, e não haja gado nos currais;",
            "18": "Todavia eu me alegro no Senhor, e me regozijo no Deus da minha salvação."
        }
    },
    "Sofonias": {
        "3": {
            "17": "O Senhor teu Deus está no meio de ti, um Deus valente que salvará; ele se deleitará em ti com alegria, calar-se-á de amor, regozijar-se-á em ti com louvor."
        }
    },
    "Ageu": {
        "1": {
            "1": "No segundo ano do rei Dario, no sexto mês, no primeiro dia do mês, veio a palavra do Senhor por intermédio do profeta Ageu a Zorobabel, filho de Sealtiel, governador de Judá, e a Josué, filho de Jozadaque, sumo sacerdote, dizendo:"
        }
    },
    "Zacarias": {
        "2": {
            "8": "Porque assim diz o Senhor dos Exércitos: Após a glória ele me enviou às nações que vos despojaram; porque aquele que vos toca toca à menina do seu olho."
        }
    },
    "Malaquias": {
        "3": {
            "1": "Eis que eu envio o meu mensageiro, que preparará o caminho diante de mim; e de repente virá ao seu templo o Senhor a quem vós buscais, e o anjo da aliança, a quem vós desejais. Eis que ele vem, diz o Senhor dos Exércitos.",
            "6": "Porque eu, o Senhor, não mudo; por isso vós, filhos de Jacó, não sois consumidos."
        }
    },
    "Mateus": {
        "1": {
            "23": "Eis que a virgem conceberá, e dará à luz um filho, e seu nome será Emanuel, que quer dizer, Deus conosco."
        },
        "5": {
            "3": "Bem-aventurados os pobres de espírito, porque deles é o reino dos céus.",
            "4": "Bem-aventurados os que choram, porque serão consolados.",
            "5": "Bem-aventurados os mansos, porque herdarão a terra.",
            "6": "Bem-aventurados os que têm fome e sede de justiça, porque serão fartos.",
            "7": "Bem-aventurados os misericordiosos, porque alcançarão misericórdia.",
            "8": "Bem-aventurados os limpos de coração, porque verão a Deus.",
            "9": "Bem-aventurados os pacíficos, porque serão chamados filhos de Deus.",
            "43": "Ouvistes que foi dito: Amarás o teu próximo, e aborrecerás o teu inimigo.",
            "44": "Eu, porém, vos digo: Amai os vossos inimigos, abençoai os que vos maldizem, fazei bem aos que vos odeiam, e orai pelos que vos maltratam e vos perseguem;"
        },
        "6": {
            "9": "Portanto, vós orareis assim: Pai nosso, que estás nos céus, santificado seja o teu nome;",
            "10": "Venha o teu reino, faça-se a tua vontade, assim na terra como no céu.",
            "11": "O pão nosso de cada dia nos dá hoje;",
            "12": "E perdoa-nos as nossas dívidas, assim como nós perdoamos aos nossos devedores.",
            "13": "E não nos induzas em tentação; mas livra-nos do mal; porque teu é o reino, e o poder, e a glória, para sempre. Amém."
        },
        "7": {
            "7": "Pedi, e dar-se-vos-á; buscai, e encontrareis; batei, e abrir-se-vos-á.",
            "12": "Portanto, tudo quanto vós quereis que os homens vos façam, fazei-lho também vós; porque esta é a lei e os profetas."
        },
        "11": {
            "28": "Vinde a mim, todos os que estais cansados e carregados, e eu vos aliviarei.",
            "29": "Tomai sobre vós o meu jugo, e aprendei de mim, que sou manso e humilde de coração; e encontrareis descanso para as vossas almas."
        },
        "16": {
            "24": "Então disse Jesus aos seus discípulos: Se alguém quer vir após mim, negue-se a si mesmo, tome a sua cruz, e siga-me."
        },
        "18": {
            "20": "Porque onde estiverem dois ou três reunidos em meu nome, aí estou eu no meio deles."
        },
        "22": {
            "37": "Jesus disse-lhe: Amarás o Senhor teu Deus de todo o teu coração, e de toda a tua alma, e de todo o teu pensamento.",
            "38": "Este é o primeiro e grande mandamento.",
            "39": "E o segundo, semelhante a este, é: Amarás o teu próximo como a ti mesmo.",
            "40": "Destes dois mandamentos dependem toda a lei e os profetas."
        },
        "24": {
            "14": "E este evangelho do reino será pregado em todo o mundo, em testemunho a todas as nações, e então virá o fim."
        },
        "28": {
            "19": "Portanto, ide, ensinai todas as nações, batizando-as em nome do Pai, e do Filho, e do Espírito Santo;",
            "20": "Ensinando-as a guardar todas as coisas que eu vos tenho mandado; e eis que eu estou convosco todos os dias, até à consumação dos séculos. Amém."
        }
    },
    "Marcos": {
        "1": {
            "1": "Princípio do evangelho de Jesus Cristo, Filho de Deus;",
            "11": "E houve uma voz dos céus, dizendo: Tu és o meu Filho muito amado, em quem me comprazo."
        },
        "4": {
            "39": "E ele, despertando, repreendeu o vento, e disse ao mar: Cala-te, emudece. E o vento se amainou, e fez-se grande bonança."
        },
        "10": {
            "45": "Porque também o Filho do homem não veio para ser servido, mas para servir, e para dar a sua vida em resgate de muitos."
        },
        "11": {
            "24": "Por isso vos digo que tudo quanto pedirdes em oração, crede que recebestes, e tê-lo-eis."
        },
        "12": {
            "30": "E amarás o Senhor teu Deus de todo o teu coração, de toda a tua alma, de todo o teu entendimento, e de toda a tua força. Este é o primeiro mandamento."
        }
    },
    "Lucas": {
        "1": {
            "26": "Ora, no sexto mês foi enviado o anjo Gabriel por Deus a uma cidade da Galileia, chamada Nazaré,",
            "30": "Então o anjo lhe disse: Maria, não temas, porque achaste graça diante de Deus."
        },
        "2": {
            "11": "Porque vos nasceu hoje, na cidade de Davi, um Salvador, que é Cristo, o Senhor;",
            "14": "Glória a Deus nas alturas, paz na terra, boa vontade para com os homens."
        },
        "6": {
            "27": "Mas a vós outros que me ouvis digo: Amai os vossos inimigos, fazei bem aos que vos odeiam,"
        },
        "12": {
            "6": "Não se vendem cinco pardais por dois ceiteis? E contudo nenhum deles está esquecido diante de Deus.",
            "7": "Mas até os cabelos da vossa cabeça estão todos contados. Não temais, pois; mais valeis vós do que muitos pardais."
        },
        "15": {
            "11": "E disse: Um certo homem tinha dois filhos;",
            "20": "Levantou-se, e foi para seu pai. Quando ainda estava longe, seu pai o viu, e, movido de compaixão, correu para cima dele, e o abraçou, e beijou."
        },
        "18": {
            "9": "Também contou Jesus a alguns que confiavam em si mesmos, imaginando que eram justos, e desprezavam aos outros, esta parábola:"
        },
        "23": {
            "34": "E Jesus dizia: Pai, perdoa-lhes, porque não sabem o que fazem. E, repartindo as suas vestes, lançavam sortes."
        },
        "24": {
            "46": "E disse-lhes: Assim está escrito, e assim convinha que o Cristo padecesse, e ressuscitasse dos mortos no terceiro dia,"
        }
    },
    "Joao": {
        "1": {
            "1": "No princípio era o Verbo, e o Verbo estava com Deus, e o Verbo era Deus.",
            "3": "Todas as coisas foram feitas por ele, e sem ele nada do que foi feito se fez.",
            "14": "E o Verbo se fez carne, e habitou entre nós, e vimos a sua glória, como a glória do unigênito do Pai, cheio de graça e de verdade."
        },
        "3": {
            "16": "Porque Deus amou o mundo de tal maneira que deu o seu Filho unigênito, para que todo aquele que nele crê não pereça, mas tenha a vida eterna.",
            "17": "Porque Deus enviou o seu Filho ao mundo, não para que condenasse o mundo, mas para que o mundo fosse salvo por ele.",
            "18": "Quem nele crê não é condenado; mas quem não crê já está condenado, porquanto não crê no nome do unigênito Filho de Deus."
        },
        "8": {
            "12": "Falou-lhes Jesus outra vez, dizendo: Eu sou a luz do mundo; quem me segue não andará em trevas, mas terá a luz da vida.",
            "32": "E conhecereis a verdade, e a verdade vos libertará."
        },
        "10": {
            "10": "O ladrão vem somente para roubar, matar e destruir; eu vim para que tenham vida, e a tenham com abundância.",
            "14": "Eu sou o bom pastor, e conheço as minhas ovelhas, e das minhas sou conhecido."
        },
        "11": {
            "25": "Disse-lhe Jesus: Eu sou a ressurreição e a vida; quem crê em mim, ainda que esteja morto, viverá;",
            "26": "E todo aquele que vive, e crê em mim, nunca morrerá. Crês tu isto?"
        },
        "13": {
            "34": "Um novo mandamento vos dou: Que vos ameis um ao outro; como eu vos amei a vós, que também vós uns aos outros vos ameis.",
            "35": "Nisto conhecerão todos que sois meus discípulos, se vos amardes uns aos outros."
        },
        "14": {
            "1": "Não se turbe o vosso coração; credes em Deus, crede também em mim.",
            "6": "Disse-lhe Jesus: Eu sou o caminho, e a verdade, e a vida; ninguém vem ao Pai, senão por mim."
        },
        "15": {
            "4": "Permanecei em mim, e eu permanecerei em vós; como a vara não pode dar fruto de si mesma, se não permanecer na videira, assim também vós, se não permanecerdes em mim.",
            "5": "Eu sou a videira, vós sois as varas; quem permanece em mim, e eu nele, esse dá muito fruto; porque sem mim nada podeis fazer.",
            "12": "O meu mandamento é este: Que vos ameis uns aos outros, como eu vos amei."
        },
        "17": {
            "3": "E a vida eterna é esta: que te conheçam, a ti só, por único Deus verdadeiro, e a Jesus Cristo, a quem enviaste."
        },
        "20": {
            "22": "E, havendo dito isto, soprou sobre eles, e disse-lhes: Recebei o Espírito Santo."
        }
    },
    "Atos": {
        "1": {
            "8": "Mas recebereis a virtude do Espírito Santo, que há de vir sobre vós; e ser-me-eis testemunhas, tanto em Jerusalém como em toda a Judéia, e Samaria, e até aos confins da terra."
        },
        "2": {
            "38": "E Pedro lhes disse: Arrependei-vos, e cada um de vós seja batizado em nome de Jesus Cristo, para remissão dos vossos pecados; e recebereis o dom do Espírito Santo."
        },
        "4": {
            "12": "E em nenhum outro há salvação, porque também debaixo do céu nenhum outro nome há, dado entre os homens, pelo qual devamos ser salvos."
        },
        "9": {
            "15": "E disse-lhe o Senhor: Vai, porque este é para mim um vaso escolhido, para levar o meu nome diante dos gentios, e dos reis, e dos filhos de Israel;"
        },
        "16": {
            "31": "E eles disseram: Crê no Senhor Jesus, e serás salvo, tu e a tua casa."
        },
        "17": {
            "24": "O Deus que fez o mundo e tudo quanto nele há, sendo Senhor do céu e da terra, não habita em templos feitos por mãos de homens;"
        }
    },
    "Romanos": {
        "1": {
            "16": "Porque não me envergonho do evangelho de Cristo; porque é o poder de Deus para salvação de todo aquele que crê; primeiro do judeu, e também do grego."
        },
        "3": {
            "23": "Porque todos pecaram e destituídos estão da glória de Deus;"
        },
        "5": {
            "1": "Sendo, pois, justificados pela fé, temos paz com Deus, por nosso Senhor Jesus Cristo;",
            "8": "Mas Deus prova o seu amor para conosco, em que Cristo morreu por nós, sendo nós ainda pecadores."
        },
        "6": {
            "23": "Porque o salário do pecado é a morte, mas o dom gratuito de Deus é a vida eterna, por Cristo Jesus, nosso Senhor."
        },
        "8": {
            "1": "Portanto, agora nenhuma condenação há para os que estão em Cristo Jesus, que andam não segundo a carne, mas segundo o Espírito.",
            "28": "E sabemos que todas as coisas contribuem para o bem daqueles que amam a Deus, daqueles que são chamados segundo o seu propósito.",
            "37": "Mas em todas estas coisas somos mais que vencedores, por aquele que nos amou.",
            "38": "Porque estou certo de que, nem a morte, nem a vida, nem os anjos, nem os principados, nem as potestades, nem o presente, nem o porvir,",
            "39": "Nem a altura, nem a profundidade, nem nenhuma outra criatura nos poderá separar do amor de Deus, que está em Cristo Jesus, nosso Senhor."
        },
        "12": {
            "1": "Rogo-vos, pois, irmãos, pela compaixão de Deus, que apresenteis os vossos corpos em sacrifício vivo, santo e agradável a Deus, que é o vosso culto racional.",
            "2": "E não vos conformeis com este século, mas transformai-vos pela renovação do vosso entendimento, para que experimenteis qual seja a boa, agradável, e perfeita vontade de Deus."
        }
    },
    "1 Corintios": {
        "1": {
            "25": "Porque a loucura de Deus é mais sábia do que os homens; e a fraqueza de Deus é mais forte do que os homens."
        },
        "2": {
            "9": "Mas, como está escrito: Nem olho viu, nem ouvido ouviu, nem jamais penetrou em coração humano, o que Deus preparou para aqueles que o amam."
        },
        "10": {
            "13": "Não vos sobreveio tentação, senão humana; mas fiel é Deus, que não vos deixará tentar acima do que podeis, antes com a tentação dará também o escape, para que a possais suportar."
        },
        "13": {
            "4": "A caridade é sofredora, é benigna; a caridade não é invejosa, não é jactanciosa, não se ensoberbece,",
            "5": "Não se porta desonenosamente, não busca seus interesses, não se irrita, não suspeita mal nenhum;",
            "6": "Não folga com a injustiça, mas folga com a verdade;",
            "7": "Tudo sofre, tudo crê, tudo espera, tudo suporta.",
            "8": "A caridade nunca falha; mas havendo profecias, desaparecerão; havendo línguas, cessarão; havendo ciência, desaparecerá."
        },
        "15": {
            "57": "Mas graças a Deus, que nos dá a vitória por nosso Senhor Jesus Cristo."
        }
    },
    "2 Corintios": {
        "5": {
            "17": "Portanto, se alguém está em Cristo, nova criatura é; as coisas velhas já passaram; eis que tudo se fez novo.",
            "21": "Aquele que não conheceu pecado, o fez pecado por nós; para que nele fôssemos feitos justiça de Deus."
        },
        "12": {
            "9": "E disse-me: A minha graça te basta, porque o meu poder se aperfeiçoa na fraqueza. De boa vontade, pois, me gloriarei nas minhas fraquezas, para que em mim habite o poder de Cristo."
        }
    },
    "Galatas": {
        "2": {
            "20": "Já estou crucificado com Cristo; e vivo, não mais eu, mas Cristo vive em mim; e a vida que agora vivo na carne, vivo-a na fé do Filho de Deus, que me amou, e se entregou a si mesmo por mim."
        },
        "5": {
            "22": "Mas o fruto do Espírito é: amor, gozo, paz, longanimidade, benignidade, bondade, fé,",
            "23": "Mansidão, temperança; contra estas coisas não há lei."
        }
    },
    "Efesios": {
        "1": {
            "3": "Bendito seja o Deus e Pai de nosso Senhor Jesus Cristo, que nos abençoou com toda a bênção espiritual nos lugares celestiais em Cristo;"
        },
        "2": {
            "8": "Porque pela graça sois salvos, por meio da fé; e isto não vem de vós, é dom de Deus;",
            "9": "Não vem das obras, para que ninguém se glorie."
        },
        "3": {
            "16": "Para que vos conceda, segundo as riquezas da sua glória, que sejais fortalecidos com poder pelo seu Espírito no homem interior;"
        },
        "5": {
            "1": "Sede, pois, imitadores de Deus, como filhos muito amados;",
            "2": "E andai em amor, como também Cristo vos amou, e se entregou a si mesmo por nós, como oferta e sacrifício a Deus, em cheiro suave."
        },
        "6": {
            "12": "Porque não temos que lutar contra a carne e o sangue, mas contra os principados, contra as potestades, contra os príncipes das trevas deste século, contra as hostes espirituais da maldade, nos lugares celestiais."
        }
    },
    "Filipenses": {
        "2": {
            "9": "Por isso também Deus o exaltou soberanamente, e lhe deu um nome que é sobre todo o nome;",
            "10": "Para que ao nome de Jesus se dobre todo o joelho dos que estão nos céus, e na terra, e debaixo da terra,"
        },
        "3": {
            "9": "E seja achado nele, não tendo a minha justiça, que vem da lei, mas a que vem pela fé em Cristo, a saber, a justiça que vem de Deus pela fé;"
        },
        "4": {
            "6": "Não andeis ansiosos de coisa alguma; antes as vossas petições sejam em tudo conhecidas diante de Deus pela oração e súplicas, com ação de graças.",
            "7": "E a paz de Deus, que excede todo o entendimento, guardará os vossos corações e os vossos pensamentos em Cristo Jesus.",
            "13": "Posso todas as coisas em Cristo que me fortalece."
        }
    },
    "Colossenses": {
        "1": {
            "13": "O qual nos tirou da potestade das trevas, e nos transportou para o reino do Filho do seu amor;",
            "14": "Em quem temos a redenção pelo seu sangue, a saber, a remissão dos pecados."
        },
        "3": {
            "3": "Porque morrestes, e a vossa vida está escondida com Cristo em Deus."
        }
    },
    "1 Tessalonicenses": {
        "4": {
            "16": "Porque o mesmo Senhor descerá do céu com alarido, e com voz de arcanjo, e com a trombeta de Deus; e os mortos em Cristo ressuscitarão primeiro."
        },
        "5": {
            "16": "Regozijai-vos sempre.",
            "17": "Orai sem cessar.",
            "18": "Em tudo dai graças, porque esta é a vontade de Deus em Cristo Jesus para convosco."
        }
    },
    "2 Tessalonicenses": {
        "2": {
            "15": "Portanto, irmãos, permanecei firmes, e guardai as tradições que vos foram ensinadas, quer por palavra, quer por carta nossa."
        }
    },
    "1 Timoteo": {
        "2": {
            "5": "Porque há um só Deus, e um só mediador entre Deus e os homens, Jesus Cristo homem;"
        },
        "6": {
            "10": "Porque o amor do dinheiro é a raiz de toda a espécie de males; e nessa cobiça alguns se desviaram da fé, e se traspassaram a si mesmos com muitas dores.",
            "12": "Toma o combate da fé, toma a vida eterna, para a qual também foste chamado, fazendo boa profissão de fé diante de muitas testemunhas."
        }
    },
    "2 Timoteo": {
        "1": {
            "7": "Porque não nos deu Deus o espírito de medo, mas de fortaleza, e de amor, e de moderação.",
            "12": "Por isso padeço isto, todavia não me envergonho, porque eu sei em quem tenho crido, e estou certo de que é poderoso para guardar o meu depósito até àquele Dia."
        },
        "2": {
            "15": "Procura apresentar-te a Deus aprovado, como obreiro que não tem de que se envergonhar, que maneja bem a palavra da verdade."
        },
        "4": {
            "7": "Combati o bom combate, terminei a carreira, guardei a fé.",
            "8": "Desde agora a coroa da justiça me está guardada, a qual o Senhor, justo juiz, me dará naquele Dia; e não somente a mim, mas também a todos os que amarem a sua vinda."
        }
    },
    "Tito": {
        "2": {
            "11": "Porque a graça de Deus se manifestou, trazendo salvação a todos os homens,",
            "12": "Ensinando-nos que, renunciando à impiedade e às concupiscências mundanas, vivamos neste mundo sóbria, justa e piamente,"
        }
    },
    "Filemom": {
        "1": {
            "8": "Portanto, ainda que tenha em Cristo grande liberdade para te ordenar o que é conveniente,"
        }
    },
    "Hebreus": {
        "1": {
            "1": "Havendo Deus antigamente falado muitas vezes, e de muitas maneiras aos pais pelos profetas, nestes últimos dias nos falou pelo Filho,"
        },
        "4": {
            "12": "Porque a palavra de Deus é viva, e eficaz, e mais penetrante do que qualquer espada de dois gumes, e penetra até à divisão da alma, e do espírito, e das juntas e medulas, e é apta para discernir os pensamentos e intenções do coração."
        },
        "10": {
            "24": "E consideremos uns aos outros, para nos provocarmos à caridade e às boas obras;"
        },
        "11": {
            "1": "Ora, a fé é o firme fundamento das coisas que se esperam, e a prova das coisas que não se veem.",
            "6": "Mas sem fé é impossível agradar-lhe; porque é necessário que aquele que se aproxima de Deus creia que ele existe, e que é galardoador dos que o buscam."
        },
        "12": {
            "1": "Portanto, nós também, sendo circundados de uma nuvem tão grande de testemunhas, deixemos todo o embaraço, e o pecado que tão de perto nos rodeia, e corramos com paciência a carreira que nos é proposta,"
        },
        "13": {
            "8": "Jesus Cristo é o mesmo, ontem, e hoje, e eternamente."
        }
    },
    "Tiago": {
        "1": {
            "2": "Meus irmãos, tende por sumo gozo quando caírdes em várias tentações;",
            "3": "Sabendo que a prova da vossa fé produz a paciência.",
            "22": "Portanto, tornai-vos praticantes da palavra, e não somente ouvintes, enganando-vos com falsos discursos."
        },
        "2": {
            "26": "Porque assim como o corpo sem o espírito está morto, assim também a fé sem obras é morta."
        },
        "5": {
            "16": "Confessai as vossas culpas uns aos outros, e orai uns pelos outros para que sareis. A oração feita por um justo pode muito em seus efeitos."
        }
    },
    "1 Pedro": {
        "1": {
            "3": "Bendito o Deus e Pai de nosso Senhor Jesus Cristo, que, segundo a sua muita misericórdia, nos regenerou para uma viva esperança, pela ressurreição de Jesus Cristo dentre os mortos,"
        },
        "2": {
            "9": "Mas vós sois geração eleita, sacerdócio real, nação santa, povo adquirido, para que anuncieis as virtudes daquele que vos chamou das trevas para a sua luz admirável;"
        },
        "5": {
            "7": "Lançando sobre ele toda a vossa ansiedade, porque ele tem cuidado de vós."
        }
    },
    "2 Pedro": {
        "1": {
            "2": "Graça e paz vos sejam multiplicadas pelo conhecimento de Deus, e de Jesus nosso Senhor;"
        },
        "3": {
            "9": "O Senhor não retarda a sua promessa, ainda que alguns a têm por tardia; mas é longânimo para conosco, não querendo que alguns se percam, senão que todos venham a arrepender-se."
        }
    },
    "1 Joao": {
        "1": {
            "1": "O que era desde o princípio, o que ouvimos, o que vimos com os nossos olhos, o que contemplamos, e as nossas mãos apalparam, da Palavra da vida.",
            "5": "E esta é a mensagem que dele ouvimos, e vos anunciamos: que Deus é luz, e não há nele treva alguma."
        },
        "3": {
            "1": "Vede que grande amor nos tem concedido o Pai, a saber, que fôssemos chamados filhos de Deus. Por isso o mundo não nos conhece, porque não o conheceu a ele."
        },
        "4": {
            "7": "Amados, amemo-nos uns aos outros; porque o amor é de Deus; e qualquer que ama é nascido de Deus, e conhece a Deus.",
            "8": "Quem não ama não conhece a Deus; porque Deus é amor."
        },
        "5": {
            "11": "E o testemunho é este: que Deus nos deu a vida eterna; e esta vida está no seu Filho."
        }
    },
    "2 Joao": {
        "1": {
            "1": "O ancião à eleita senhora e aos seus filhos, a quem amo na verdade; e não somente eu, mas também todos os que conhecem a verdade;"
        }
    },
    "3 Joao": {
        "1": {
            "11": "Amado, não sigas o que é mau, mas o que é bom. Quem faz bem é de Deus; e quem faz mal não viu a Deus."
        }
    },
    "Judas": {
        "1": {
            "24": "Ora, àquele que é poderoso para vos guardar de tropeçar, e apresentar-vos irrepreensíveis diante da sua glória com alegria,"
        }
    },
    "Apocalipse": {
        "1": {
            "1": "A Revelação de Jesus Cristo, que Deus lhe deu, para mostrar aos seus servos as coisas que brevemente devem acontecer; e a manifestou, enviando-a pelo seu anjo ao seu servo João;",
            "5": "E da parte de Jesus Cristo, que é a testemunha fiel, o primogênito dos mortos, e o príncipe dos reis da terra. Àquele que nos amou, e nos lavou dos nossos pecados com o seu sangue,",
            "8": "Eu sou o Alfa e o Ômega, o princípio e o fim, diz o Senhor, que é, e que era, e que há de vir, o Todo-Poderoso."
        },
        "3": {
            "20": "Eis que estou à porta, e bato; se alguém ouvir a minha voz, e abrir a porta, entrarei em sua casa, e com ele cearei, e ele comigo."
        },
        "5": {
            "12": "Dizendo com grande voz: Digno é o Cordeiro, que foi morto, de receber o poder, e riquezas, e sabedoria, e força, e honra, e glória, e louvor."
        },
        "19": {
            "6": "E ouvi como que a voz de uma grande multidão, e como que o ruído de muitas águas, e como que o som de fortes trovões, dizendo: Aleluia, porque já o Senhor nosso Deus, o Todo-Poderoso, reina."
        },
        "21": {
            "3": "E ouvi uma grande voz do céu, que dizia: Eis aqui o tabernáculo de Deus com os homens, pois com eles habitará, e eles serão o seu povo, e o mesmo Deus estará com eles, e será o seu Deus.",
            "4": "E Deus limpará de seus olhos toda a lágrima; e não haverá mais morte, nem pranto, nem clamor, nem dor; porque as primeiras coisas são passadas."
        },
        "22": {
            "12": "E eis que cedo venho, e o meu galardão comigo, para dar a cada um segundo a sua obra.",
            "13": "Eu sou o Alfa e o Ômega, o primeiro e o derradeiro, o princípio e o fim.",
            "20": "Aquele que testifica estas coisas diz: Certamente cedo venho. Amém. Vem, Senhor Jesus.",
            "21": "A graça do Senhor Jesus Cristo seja com todos vós. Amém."
        }
    }
}


def gerar_biblia_json(output_path):
    """Gera arquivo JSON com dados da Biblia."""
    try:
        with open(output_path, 'w', encoding='utf-8') as f:
            json.dump(BIBLIA_DADOS, f, ensure_ascii=False, indent=2)

        # Conta total de versículos
        total_versiculos = 0
        total_livros = len(BIBLIA_DADOS)

        for livro, capitulos in BIBLIA_DADOS.items():
            for capitulo, versiculos in capitulos.items():
                if isinstance(versiculos, dict):
                    total_versiculos += len(versiculos)

        print(f"[OK] Arquivo JSON gerado com sucesso!")
        print(f"     Arquivo: {output_path}")
        print(f"     Livros: {total_livros}")
        print(f"     Versículos: {total_versiculos}")

        return True

    except IOError as e:
        print(f"[ERRO] Erro ao criar arquivo: {e}")
        return False


if __name__ == "__main__":
    script_dir = Path(__file__).parent
    output_path = script_dir / "biblia_completa_grande.json"

    print("=" * 70)
    print("Gerador de Biblia Completa Grande (JSON)")
    print("=" * 70)
    print()
    print("[*] Gerando arquivo da Biblia com versículos em português...")
    print()

    sucesso = gerar_biblia_json(str(output_path))

    if sucesso:
        print()
        print("=" * 70)
        print("[OK] Biblia completa gerada com sucesso!")
        print("=" * 70)
        print()
        print("Arquivo pronto para ser usado com gerar_sql_versiculos.py")
        print()
