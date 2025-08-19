import React from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import Container from "@core/Container/Container";
import Headings from "@core/Text/Headings";
import { useUser } from "@hooks/UserProvider";
import Paragraph from "@core/Text/Paragraph";
import { FaUser } from "react-icons/fa";

export default function Nav({ title = "ISNab" }) {
	const location = useLocation();
	const items = [{"label" : "Produkty", "to" : "/produkty"}, {"label" : "Objednávky", "to" : "/objednavky"}] // tady podle role udělat nav
	const navigate = useNavigate();
	const { user } = useUser();


	return (
		<Container property={"w-full bg-blue-300"}>
			<Container property={"max-w-7xl mx-auto px-4"}>
				<Container property={"flex items-center justify-between h-12"}>
					<Container property={"flex items-center gap-4"}>
						<Container  property="p-4 cursor-pointer"  onClick={() => navigate('/produkty')}>
							<Headings sizeTag="h4" property={"text-white"}>{title}</Headings>
						</Container>

						{user.hasData() && (
							<Container property="hidden md:flex items-center gap-3 bg-white bg-opacity-20 px-2 rounded-lg">
								<FaUser className="text-white"/>
								<Container property="flex flex-col leading-tight">
									<Paragraph property="text-white text-sm font-medium">{user.username}</Paragraph>
									<Paragraph property="text-white text-xs opacity-80">{user.role}</Paragraph>
								</Container>
							</Container>
						)} 
					</Container>

					<Container property={"flex items-center gap-6"}>
						{items.map((item) => {
							const to = item?.to || "#";
							const label = item?.label || to;
							const isActive = location.pathname === to;
							return (
								<Link
									key={`${label}-${to}`}
									to={to}
									className={`transition-colors ${
										isActive
											? "text-blue-800 border-b-2 border-blue-800"
											: "text-white hover:text-gray-200"
									}`}
								>
									{label}
								</Link>
							);
						})}
					</Container>
				</Container>
			</Container>
		</Container>
	);
}



