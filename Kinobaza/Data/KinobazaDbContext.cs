﻿using Kinobaza.Models;
using Microsoft.EntityFrameworkCore;

namespace Kinobaza.Data
{
    public class KinobazaDbContext : DbContext
    {
        public DbSet<Genre> Genres {  get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Record> Records { get; set; }
        public KinobazaDbContext(DbContextOptions<KinobazaDbContext> options) : base(options)
        {
            if (Database.EnsureCreated())
            {
                User admin = new()
                {
                    Login = "admin",
                    Email = "admin@gmail.com",
                    Password = "D5CDEDF54566480FADA61635BB7D5A25",
                    Salt = "943078C0E5F373684F8C23DF51B2E9F4",
                    Status = null
                };
                User user1 = new()
                {
                    Login = "user1",
                    Email = "user1@gmail.com",
                    Password = "1655CCFC58214870FA45B2D346FC4640",
                    Salt = "1618CC3D9CAE3971D33680FC1DCA5F24",
                    Status = "waiting"
                };
                User user2 = new()
                {
                    Login = "user2",
                    Email = "user2@gmail.com",
                    Password = "AA135297410B66DB8C5E51AF04F19357",
                    Salt = "DD1E12BBA66955E43B531D9F56065526",
                    Status = "waiting"
                };
                User user3 = new()
                {
                    Login = "user3",
                    Email = "user3@gmail.com",
                    Password = "DA227DDBAD0AFFEE7D54702CC56CA1AE",
                    Salt = "B9BFBE92C051BD486783AED7F4DDDBAF",
                    Status = "waiting"
                };
                User user4 = new()
                {
                    Login = "user4",
                    Email = "user4@gmail.com",
                    Password = "63808136E2A1B4BAA08242AFF74ED0F4",
                    Salt = "3CD88D20C4262D659756A345DBDB1C2B",
                    Status = "waiting"
                };
                User user5 = new()
                {
                    Login = "user5",
                    Email = "user5@gmail.com",
                    Password = "14EA9DCFB768B6B87409B134E8408428",
                    Salt = "DC0607E3E58A418EBF866E9CA02A05D2",
                    Status = "waiting"
                };
                User user6 = new()
                {
                    Login = "user6",
                    Email = "user6@gmail.com",
                    Password = "FB7C17554C85FDBFAEE951B452BB7116",
                    Salt = "9EE056E0E74B7DCD99245CF0860D8566",
                    Status = "waiting"
                };
                User user7 = new()
                {
                    Login = "user7",
                    Email = "user7@gmail.com",
                    Password = "9C8F9113D6EC7873D23EBAA7B93234CA",
                    Salt = "A7FFD3B31B453B8C9CCC41577BBF6BF2",
                    Status = "waiting"
                };
                User user8 = new()
                {
                    Login = "user8",
                    Email = "user8@gmail.com",
                    Password = "2681B8E55367F312596CDCA0DD72457D",
                    Salt = "FFADE8DADB60AA7503A2C733B8017788",
                    Status = "waiting"
                };
                User user9 = new()
                {
                    Login = "user9",
                    Email = "user9@gmail.com",
                    Password = "C13AC2AAB72323436703D25EADDB9149",
                    Salt = "ED2BB4E16CF6BB11E9A8F52441E60DEF",
                    Status = "waiting"
                };
                User user10 = new()
                {
                    Login = "user10",
                    Email = "user10@gmail.com",
                    Password = "376A7518ACCC5FF816118DB32E03D8C0",
                    Salt = "9A3381A07387F65E5CDFDEB1BA50446D",
                    Status = "waiting"
                };

                Users?.AddRange(admin, user1, user2, user3, user4, user5, user6, user7, user8, user9, user10);

                Genre drama = new() { Name = "Драма" };
                Genre adventure = new() { Name = "Приключение" };
                Genre action = new() { Name = "Боевик" };
                Genre historical = new() { Name = "Исторический" };
                Genre comedy = new() { Name = "Комедия" };
                Genre crime = new() { Name = "Криминал" };
                Genre horror = new() { Name = "Ужасы" };
                Genre romance = new() { Name = "Романтика" };
                Genre thriller = new() { Name = "Триллер" };
                Genre animation = new() { Name = "Аннимационный" };
                Genre family = new() { Name = "Семейный" };
                Genre fantasy = new() { Name = "Фэнтези" };
                Genre war = new() { Name = "Военный" };
                Genre documentary = new() { Name = "Документальный" };
                Genre musical = new() { Name = "Мюзикл" };
                Genre biography = new() { Name = "Биографический" };
                Genre sci_fi = new() { Name = "Научная фантастика" };
                Genre western = new() { Name = "Вестерн" };
                Genre post_apocaliptic = new() { Name = "Постапокалипсический" };
                Genre cartoon = new() { Name = "Мультфильм" };

                Genres?.AddRange(drama, adventure, action, historical, comedy, crime, horror, romance,
                    thriller, animation, family, war, documentary, musical, biography, sci_fi, western,
                    post_apocaliptic, fantasy, cartoon);

                Movie _12YearsASlave = new()
                {
                    Image = @"\images\movies\12_years_a_slave.jpg",
                    TitleRU = "12 Лет Рабства",
                    TitleEN = "12 Years a Slave",
                    PremiereDate = new DateTime(2013, 8, 30),
                    Director = "Стив МакКуин",
                    Cast = "Чиветель Эджиофор, Майкл Фассбендер, Бенедикт Камбербэтч, Пол Дано, Пол Джаматти, Люпита Нионго, Сара Полсон",
                    Genres = new List<Genre>() { drama, historical },
                    Description = "У Соломона была прекрасная жизнь. У него была жена, имел образование и работал. Но однажды ему предлагают двое мужчин очень хорошую работу в Вашингтоне. Надеясь, что жизнь станет еще лучше, он принимает их предложение. К сожалению, все это оказывается проста обман. Теперь он становится рабом и работал для состоятельных людей.",
                };

                Movie _alien = new()
                {
                    Image = @"\images\movies\alien.jpg",
                    TitleRU = "Чужой",
                    TitleEN = "Alien",
                    PremiereDate = new DateTime(1979, 5, 25),
                    Director = "Ридли Скотт",
                    Cast = "Сигурни Уивер, Том Скеррит, Иэн Холм, Джон Хёрт, Гарри Дин Стэнтон, Вероника Картрайт, Яфет Котто, Боладжи Бадеджо",
                    Genres = new List<Genre>() { horror, sci_fi },
                    Description = "На Землю возвращается грузовое космическое судно. Внезапно оно получило загадочный сигнал, отправляемый неизвестной планетой. Экипаж решает экстренно сменить курс, приземлиться и попробовать разобраться, откуда поступает информация. Попав на незнакомую планету, герои-астронавты натыкаются на неопознанные объекты, которые внешне очень напоминают громадные коконы. Что может вылупиться из них? Удачным ли было решение совершить посадку?",
                };

                Movie _backToTheFuture = new()
                {
                    Image = @"\images\movies\back_to_the_future.jpg",
                    TitleRU = "Назад в будущее",
                    TitleEN = "Back to the Future",
                    PremiereDate = new DateTime(1985, 7, 3),
                    Director = "Роберт Земекис",
                    Cast = "Майкл Дж. Фокс, Кристофер Ллойд, Лиа Томпсон, Криспин Гловер, Томас Ф. Уилсон, Клодия Уэллс, Марк МакКлюр",
                    Genres = new List<Genre>() { sci_fi, adventure, comedy },
                    Description = "Фильм является первой частью научно-фантастической франшизы, повествующей о путешествиях по пространственно-временному континууму. Все начинается с момента, когда чудаковатый ученый Эммет Браун, показывает своему юному товарищу Марти, настоящую машину времени. Но их задушевную беседу внезапно прерывают бандиты, которые охотятся за инновационной разработкой. В ходе потасовки Док умирает, а Макфлаю чудом удается спастись, совершив скачок на тридцать лет назад в 50-е года. Здесь он сталкивается с чередой новых проблем – появление главного героя помешало его молодым родителям встретиться, а значит, он скоро исчезнет. Чтобы исправить грядущие и нынешние события, парню необходимо собственноручно свести маму с папой, найти молодого Дока и вернуться в будущее…",
                };
                Movie _braveheart = new()
                {
                    Image = @"\images\movies\Braveheart.jpg",
                    TitleRU = "Храброе сердце",
                    TitleEN = "Braveheart",
                    PremiereDate = new DateTime(1995, 5, 18),
                    Director = "Мэл Гибсон",
                    Cast = "Мэл Гибсон, Софи Марсо, Патрик МакГуэн, Энгус МакФадьен, Брендан Глисон, Катрин МакКормак, Брайан Кокс",
                    Genres = new List<Genre>() { drama, romance },
                    Description = "Действие фильма начинается в 1280 году в Шотландии. Это история легендарного национального героя Уильяма Уолласа, посвятившего себя борьбе с англичанами при короле Эдварде Длинноногом.\r\nОн рано лишился отца, погибшего от рук англичан, и его забрал к себе дядя Оргайл, который дал ему хорошее образование в Европе. На родину Уильям возвращается уже взрослым человеком, мечтающем завести семью и жить мирной жизнью.\r\nНо судьба распорядилась иначе. Его невесту убили англичане, и он начал свой крестовый поход за свободу.",
                };
                Movie _ElLaberintoDelFauno = new()
                {
                    Image = @"\images\movies\Pans_Labyrinth.jpg",
                    TitleRU = "Лабиринт Фавна",
                    TitleEN = "Pan's Labyrinth",
                    PremiereDate = new DateTime(2006, 5, 27),
                    Director = "Гильермо дель Торо",
                    Cast = "Ивана Бакеро, Серхи Лопес, Марибель Верду, Даг Джонс, Ариадна Хиль, Алекс Ангуло, Маноло Соло, Сезар Ви",
                    Genres = new List<Genre>() { drama, fantasy, war },
                    Description = "Испания, 1944 год. Группа повстанцев сражается с фашистами в горных лесах северной Наварры. Офелия — 10-летняя одинокая и мечтательная девочка — переезжает со своей беременной матерью Кармен в военный лагерь отчима капитана Видаля. Видаль — высокомерный и жестокий офицер армии Франко, который должен очистить район от повстанцев любой ценой. Офелия, увлеченная волшебными сказками, обнаруживает старинный заброшенный лабиринт неподалеку от дома.\r\nФея проводит ее в центр лабиринта, где она встречает Фавна, хозяина подземелья. Фавн утверждает, что знает её истинную судьбу и её тайное предназначение. Оказывается Офелия — пропавшая принцесса волшебного королевства, которую многие века разыскивает ее отец. Фавн предлагает ей возможность вернуться в волшебное королевство. Но для начала она должна пройти три испытания до наступления полнолуния. Никто не должен узнать об этом.\r\nВремя на исходе — как для Офелии, так и для отряда повстанцев. Всем придется противостоять трудностям и жестокости на пути к свободе.",
                };
                Movie _findingNemo = new()
                {
                    Image = @"\images\movies\finding_Nemo.jpg",
                    TitleRU = "В поисках Немо",
                    TitleEN = "Finding Nemo",
                    PremiereDate = new DateTime(2003, 5, 18),
                    Director = "Эндрю Стэнтон, Ли Анкрич",
                    Cast = "Альберт Брукс, Эллен ДеДженерес, Александр Гоулд, Уиллем Дефо, Эллисон Дженни, Джо Рэнфт, Джеффри Раш",
                    Genres = new List<Genre>() { cartoon, comedy, adventure },
                    Description = "Немо – единственный ребенок Марлина – рыбы-клоуна. В океане так много опасностей, от которых отец никак не может оградить Немо, а он к тому же такой любознательный и любопытный. Однажды Немо все же сбежал, и отец его четко решает отправиться на поиски. Самому путешествовать по океану опасно, и он просит помощи у королевской рыбы. Приключения, полные опасностей и курьезов, начинаются.",
                };
                Movie _forrestGump = new()
                {
                    Image = @"\images\movies\forrest_gump.jpg",
                    TitleRU = "Форрест Гамп",
                    TitleEN = "Forrest Gump",
                    PremiereDate = new DateTime(1994, 6, 23),
                    Director = "Роберт Земекис",
                    Cast = "Том Хэнкс, Робин Райт, Гэри Синиз, Майкелти Уильямсон, Салли Филд, Ребекка Уильямс, Майкл Коннер Хэмпфри, Хэйли Джоэл",
                    Genres = new List<Genre>() { drama },
                    Description = "Эту историю рассказывает сам Форрест Гамп, слабоумный, безобидный человек с любящим сердцем. В этом истории рассказывается его необычная жизнь.\r\nНеобъяснимым, фантастическим образом Форрест Гамп становится то футболистом, то героем войны, то успешным бизнесменом. Он становится богатым, но он такой же глупый и добрый человек. Форрест успешен во всем, но он любит девочку, с которой он дружил в детстве, но взаимность приходит поздно.",
                };
                Movie _gladiator = new()
                {
                    Image = @"\images\movies\gladiator.jpg",
                    TitleRU = "Гладитор",
                    TitleEN = "Gladiator",
                    PremiereDate = new DateTime(2000, 5, 1),
                    Director = "Ридли Скотт",
                    Cast = "Рассел Кроу, Хоакин Феникс, Конни Нильсен, Оливер Рид, Ричард Харрис, Дерек Джекоби, Джимон Хонсу, Дэвид Скофилд",
                    Genres = new List<Genre>() { drama, action, adventure },
                    Description = "Действия фильма «Гладиатор» происходят в Древнем Риме. Тогда правили честь и гордость. Во времена великой экспансии перед этой державой на колени падали все царства. И не было в Риме более талантливого полководца, нежели Максимус. Но к сожалению, благородство есть не у каждого. Все меняется. Старый император в Максимусе души не чаял, но после его смерти сын правителя решает избавиться от соперника. Все указывает на то, что Максимус должен исчезнуть куда-нибудь. А еще лучше – умереть, предварительно доставив удовольствие толпе безумцев своим сражением на арене. Злодей не учел только того, что Максимус не позволит себе сдаться, пока не отомстит за произошедшее…",
                };
                Movie _inception = new()
                {
                    Image = @"\images\movies\inception.jpg",
                    TitleRU = "Начало",
                    TitleEN = "Inception",
                    PremiereDate = new DateTime(2010, 7, 8),
                    Director = "Кристофер Нолан",
                    Cast = "Леонардо ДиКаприо, Джозеф Гордон-Левитт, Эллен Пейдж, Кен Ватанабе, Марион Котийяр, Том Харди, Киллиан Мёрфи, Том Беренджер, Майкл Кейн",
                    Genres = new List<Genre>() { drama, action, sci_fi, thriller },
                    Description = "Кобб – вор, у которого есть одно искусство: он крадет информацию из подсознания человека, когда они спят. Он стал одним из лучших шпионов, но он же сам потом стал беглецом и потерял все, что он любил. Но у Кобба появляется шанс и он получает особенное задание – он вместо того, чтобы украсть информацию, теперь должен внедрить ее. Если это у него получится, то это будет его самым лучшим преступлением. Но ему мешает один враг, которого может увидеть только сам Кобб.",
                };
                Movie _insideOut = new()
                {
                    Image = @"\images\movies\inside_out.jpg",
                    TitleRU = "Головоломка",
                    TitleEN = "Inside Out",
                    PremiereDate = new DateTime(2015, 5, 18),
                    Director = "Пит Доктер, Роналдо Дель Кармен",
                    Cast = "Эми Полер, Дайан Лэйн, Кайл МакЛоклен, Минди Кейлинг, Билл Хейдер, Филлис Смит, Льюис Блэк, Кэтлин Диас",
                    Genres = new List<Genre>() { comedy, cartoon },
                    Description = "Райли 11 лет, и она обычная школьница, руководствующая в своей жизни несколькими основными эмоциями. До поры до времени эти эмоции помогали ей развиваться, решать проблемы, делать правильный выбор. Все нарушилось, когда девочке и ее семье пришлось переезжать в шумный мегаполис. Каждая из эмоций, которые всегда умели уживаться друг с другом, решила, что она знает, как правильно вести себя в данной ситуации, лучше других. В голове Райли творится настоящий хаос. После переезда девочке придется потрудиться, чтобы как-то начать новую жизнь и завести друзей.",
                };

                Movies?.AddRange(_12YearsASlave, _alien, _backToTheFuture, _braveheart, _ElLaberintoDelFauno,
                    _findingNemo, _forrestGump, _gladiator, _inception, _insideOut);

                SaveChanges();
            }
        }
    }
}
